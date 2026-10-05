import 'dart:convert';

import 'package:http/http.dart' as http;

import '../models/customer_account.dart';
import '../models/coverage_status.dart';
import '../models/city_option.dart';
import '../models/engineer_document.dart';
import '../models/equipment_modality.dart';
import '../models/engineer_account.dart';
import '../models/manufacture.dart';
import '../models/state_option.dart';
import 'document_file_saver.dart';
import '../utils/app_config.dart';

class ApiException implements Exception {
  const ApiException(this.message);

  final String message;

  @override
  String toString() => message;
}

class ApiService {
  ApiService({http.Client? client}) : _client = client ?? http.Client();

  final http.Client _client;

  Map<String, String> get _headers {
    final credentials = base64Encode(
      utf8.encode('${AppConfig.apiUsername}:${AppConfig.apiPassword}'),
    );
    return {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'Authorization': 'Basic $credentials',
    };
  }

  Future<List<CustomerAccount>> getCustomers() async {
    final rows = await _getList('/api/customer-equipment-registrations');
    return rows.map(CustomerAccount.fromJson).toList();
  }

  Future<List<EngineerAccount>> getEngineers() async {
    final rows = await _getList('/api/field-service-engineers');
    return rows.map(EngineerAccount.fromJson).toList();
  }

  Future<List<EquipmentModality>> getEquipmentModalities() async {
    final rows = await _getList('/api/equipment-modalities');
    return rows.map(EquipmentModality.fromJson).toList();
  }

  Future<List<Manufacture>> getManufactures() async {
    final rows = await _getList('/api/manufactures');
    return rows.map(Manufacture.fromJson).toList();
  }

  Future<List<CoverageStatus>> getCoverageStatuses() async {
    final rows = await _getList('/api/coverage-statuses');
    return rows.map(CoverageStatus.fromJson).toList();
  }

  Future<List<StateOption>> getStates() async {
    final rows = await _getList('/api/states');
    return rows.map(StateOption.fromJson).toList();
  }

  Future<List<CityOption>> getCitiesForStates(List<int> stateIds) async {
    if (stateIds.isEmpty) return const [];
    final query = stateIds.map((id) => 'stateIds=$id').join('&');
    final uri = Uri.parse('${AppConfig.apiBaseUrl}/api/cities')
        .replace(query: query);
    final rows = await _getList(uri.toString());
    return rows.map(CityOption.fromJson).toList();
  }

  // These routes create equipment registrations and engineer records,
  // respectively; they are not dedicated account-registration APIs.
  Future<void> registerCustomer(Map<String, dynamic> request) async {
    await _post('/api/customer-equipment-registrations', request);
  }

  Future<void> registerEngineer(Map<String, dynamic> request) async {
    await _post('/api/field-service-engineers', request);
  }

  Future<List<EngineerDocument>> getEngineerDocuments(
    int fieldServiceEngineerId,
  ) async {
    final rows = await _getList(
      '/api/field-service-engineers/$fieldServiceEngineerId/documents',
    );
    return rows.map(EngineerDocument.fromJson).toList(growable: false);
  }

  Future<bool> downloadEngineerDocument({
    required int fieldServiceEngineerId,
    required EngineerDocument document,
  }) async {
    final response = await _client.get(
      Uri.parse(
        '${AppConfig.apiBaseUrl}/api/field-service-engineers/'
        '$fieldServiceEngineerId/documents/${document.id}/download',
      ),
      headers: _headers,
    );
    if (response.statusCode < 200 || response.statusCode >= 300) {
      _decodeResponse(response);
      throw ApiException('Could not download ${document.originalFileName}.');
    }

    return saveDocumentBytes(
      fileName: document.originalFileName,
      contentType: document.contentType ?? 'application/octet-stream',
      bytes: response.bodyBytes,
    );
  }

  Future<CustomerAccount> loginCustomer(String email, String password) async {
    final response = await _post(
      '/api/customer-equipment-registrations/login',
      {'email': email, 'password': password},
    );
    return CustomerAccount.fromJson(_responseData(response));
  }

  Future<EngineerAccount> loginEngineer(String email, String password) async {
    final response = await _post('/api/field-service-engineers/login', {
      'email': email,
      'password': password,
    });
    return EngineerAccount.fromJson(_responseData(response));
  }

  Future<List<Map<String, dynamic>>> _getList(String path) async {
    final uri = path.startsWith('http')
        ? Uri.parse(path)
        : Uri.parse('${AppConfig.apiBaseUrl}$path');
    final response = await _client.get(uri, headers: _headers);
    final decoded = _decodeResponse(response);
    dynamic data = decoded;
    if (decoded is Map<String, dynamic> && decoded.containsKey('data')) {
      data = decoded['data'];
    }
    if (data is! List) {
      throw const ApiException('The API returned an unexpected list response.');
    }
    return data.whereType<Map<String, dynamic>>().toList(growable: false);
  }

  Future<dynamic> _post(String path, Map<String, dynamic> request) async {
    final response = await _client.post(
      Uri.parse('${AppConfig.apiBaseUrl}$path'),
      headers: _headers,
      body: jsonEncode(request),
    );
    return _decodeResponse(response);
  }

  Map<String, dynamic> _responseData(dynamic response) {
    if (response is Map<String, dynamic> &&
        response['data'] is Map<String, dynamic>) {
      return response['data'] as Map<String, dynamic>;
    }
    throw const ApiException('The API returned an unexpected login response.');
  }

  dynamic _decodeResponse(http.Response response) {
    dynamic body;
    if (response.body.isNotEmpty) {
      try {
        body = jsonDecode(response.body);
      } on FormatException {
        body = response.body;
      }
    }

    if (response.statusCode < 200 || response.statusCode >= 300) {
      if (response.statusCode == 401 || response.statusCode == 403) {
        if (body is Map<String, dynamic> && body['message'] is String) {
          throw ApiException(body['message'] as String);
        }
        throw const ApiException(
          'API access was denied. Check the Basic Auth settings in app_config.dart.',
        );
      }
      throw ApiException(_errorMessage(response.statusCode, body));
    }
    return body;
  }

  String _errorMessage(int statusCode, dynamic body) {
    if (body is Map<String, dynamic>) {
      final errors = body['errors'];
      if (errors is Map) {
        final messages = errors.values
            .expand((value) => value is List ? value : [value])
            .join('\n');
        if (messages.isNotEmpty) return messages;
      }
      if (errors is List && errors.isNotEmpty) return errors.join('\n');
      final message = body['message'] ?? body['title'];
      if (message is String && message.isNotEmpty) return message;
    }
    return 'Request failed (HTTP $statusCode). Check the API address and try again.';
  }
}

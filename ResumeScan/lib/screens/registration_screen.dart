import 'dart:convert';

import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';

import '../models/city_option.dart';
import '../models/coverage_status.dart';
import '../models/equipment_modality.dart';
import '../models/manufacture.dart';
import '../models/state_option.dart';
import '../models/user_role.dart';
import '../services/api_service.dart';
import '../widgets/app_text_field.dart';
import '../widgets/loading_indicator.dart';
import 'login_screen.dart';

class _EngineerDocumentItem {
  const _EngineerDocumentItem({
    required this.documentType,
    required this.originalFileName,
    required this.contentType,
    required this.fileSizeBytes,
    required this.fileContentBase64,
  });

  final String documentType;
  final String originalFileName;
  final String contentType;
  final int fileSizeBytes;
  final String fileContentBase64;
}

class RegistrationScreen extends StatefulWidget {
  const RegistrationScreen({super.key});

  @override
  State<RegistrationScreen> createState() => _RegistrationScreenState();
}

class _RegistrationScreenState extends State<RegistrationScreen> {
  static const int _maxDocumentSizeBytes = 10 * 1024 * 1024;
  static const int _maxTotalDocumentSizeBytes = 20 * 1024 * 1024;

  final _formKey = GlobalKey<FormState>();
  final _api = ApiService();
  final Map<String, TextEditingController> _fields = {};
  final TextEditingController _documentTypeController = TextEditingController(
    text: 'Training',
  );
  UserRole _role = UserRole.customer;
  bool _isLoading = false;
  bool _isLoadingReferenceData = true;
  int? _selectedEquipmentModalityId;
  int? _selectedManufactureId;
  int? _selectedCoverageStatusId;
  String? _selectedEmploymentStatus;
  String? _error;
  final Set<int> _selectedEngineerModalityIds = {};
  final Set<int> _selectedEngineerManufactureIds = {};
  final Set<int> _selectedEngineerStateIds = {};
  final Set<int> _selectedEngineerCityIds = {};
  final List<_EngineerDocumentItem> _documents = [];
  int _cityRequestId = 0;
  bool _isLoadingCities = false;

  final Map<String, String> _customerFields = const {
    'facilityName': 'Facility Name',
    'customerContactName': 'Customer Contact Name',
    'customerContactMobile': 'Customer Contact Mobile',
    'primaryContactEmail': 'Primary Contact Email',
    'facilityAddress': 'Facility Address',
    'equipmentModalityId': 'Equipment Modality ID',
    'manufactureId': 'Manufacture ID',
    'modelIdentifier': 'Model Identifier',
    'equipmentSerialNumber': 'Equipment Serial Number',
    'softwareFirmwareVersion': 'Software/Firmware Version (optional)',
    'coverageStatusId': 'Coverage Status ID',
    'password': 'Password',
    'confirmPassword': 'Confirm Password',
  };

  final Map<String, String> _engineerFields = const {
    'engineerName': 'Engineer Name',
    'baseLocation': 'Base Location',
    'mobileNumber': 'Mobile Number',
    'emailAddress': 'Email Address',
    'password': 'Password',
    'confirmPassword': 'Confirm Password',
  };

  @override
  void initState() {
    super.initState();
    _loadReferenceData();
  }

  List<EquipmentModality> _equipmentModalities = const [];
  List<Manufacture> _manufactures = const [];
  List<CoverageStatus> _coverageStatuses = const [];
  List<StateOption> _states = const [];
  List<CityOption> _cities = const [];

  Future<void> _loadReferenceData() async {
    try {
      final results = await Future.wait([
        _api.getEquipmentModalities(),
        _api.getManufactures(),
        _api.getCoverageStatuses(),
        _api.getStates(),
      ]);
      if (!mounted) return;
      setState(() {
        _equipmentModalities = (results[0] as List<EquipmentModality>)
            .where((modality) => modality.isEnabled)
            .toList();
        _manufactures = (results[1] as List<Manufacture>)
            .where((manufacture) => manufacture.isEnabled)
            .toList();
        _coverageStatuses = (results[2] as List<CoverageStatus>)
            .where((status) => status.isEnabled)
            .toList();
        _states =
            (results[3] as List<StateOption>)
                .where((state) => state.isEnabled)
                .toList()
              ..sort(
                (a, b) => a.name.toLowerCase().compareTo(b.name.toLowerCase()),
              );
        _isLoadingReferenceData = false;
      });
    } on ApiException catch (error) {
      if (mounted) {
        setState(() {
          _isLoadingReferenceData = false;
          _error = error.message;
        });
      }
    } catch (error) {
      if (mounted) {
        setState(() {
          _isLoadingReferenceData = false;
          _error = 'Could not load registration options: $error';
        });
      }
    }
  }

  @override
  void dispose() {
    for (final controller in _fields.values) {
      controller.dispose();
    }
    _documentTypeController.dispose();
    super.dispose();
  }

  TextEditingController _controller(String key) =>
      _fields.putIfAbsent(key, TextEditingController.new);

  String? _validate(String key, String? value) {
    final isEmpty = value == null || value.trim().isEmpty;
    if (isEmpty && key != 'softwareFirmwareVersion') {
      return 'This field is required.';
    }
    if (isEmpty) return null;

    if (key == 'engineerName' ||
        key == 'baseLocation' ||
        key == 'facilityName' ||
        key == 'customerContactName' ||
        key == 'facilityAddress') {
      if (value.trim().length < 2) return 'Enter a valid value.';
    }

    if ((key == 'email' ||
            key == 'emailAddress' ||
            key == 'primaryContactEmail') &&
        !RegExp(r'^[^\s@]+@[^\s@]+\.[^\s@]+$').hasMatch(value.trim())) {
      return 'Enter a valid email address.';
    }

    if (key == 'mobileNumber' &&
        !RegExp(r'^[0-9+()\-\s]{10,15}$').hasMatch(value.trim())) {
      return 'Enter a valid mobile number.';
    }

    if (key == 'password' && value.length < 8) {
      return 'Use at least 8 characters.';
    }

    if (key == 'confirmPassword' && value != _controller('password').text) {
      return 'Passwords do not match.';
    }

    if ({
          'equipmentModalityId',
          'manufactureId',
          'coverageStatusId',
        }.contains(key) &&
        int.tryParse(value.trim()) == null) {
      return 'Enter a valid whole number.';
    }

    return null;
  }

  String? _validateEngineerForm() {
    if (_selectedEmploymentStatus == null ||
        _selectedEmploymentStatus!.isEmpty) {
      return 'Please select the employment status.';
    }
    if (_selectedEngineerModalityIds.isEmpty ||
        _selectedEngineerManufactureIds.isEmpty) {
      return 'Add at least one equipment modality and manufacturer.';
    }
    if (_selectedEngineerStateIds.isEmpty || _selectedEngineerCityIds.isEmpty) {
      return 'Select at least one operating state and city.';
    }
    if (_documents.isEmpty) {
      return 'Upload at least one document.';
    }
    return null;
  }

  Future<void> _showEquipmentModalityPicker() async {
    final selectedIds = Set<int>.from(_selectedEngineerModalityIds);
    final result = await showDialog<Set<int>>(
      context: context,
      builder: (dialogContext) => StatefulBuilder(
        builder: (context, setDialogState) => AlertDialog(
          title: const Text('Select Equipment Modality'),
          content: SizedBox(
            width: 360,
            height: 280,
            child: ListView(
              children: _equipmentModalities
                  .map(
                    (item) => CheckboxListTile(
                      contentPadding: EdgeInsets.zero,
                      title: Text(item.name),
                      value: selectedIds.contains(item.id),
                      onChanged: (checked) => setDialogState(() {
                        if (checked == true) {
                          selectedIds.add(item.id);
                        } else {
                          selectedIds.remove(item.id);
                        }
                      }),
                    ),
                  )
                  .toList(),
            ),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(dialogContext).pop(),
              child: const Text('CANCEL'),
            ),
            TextButton(
              onPressed: () => Navigator.of(dialogContext).pop(selectedIds),
              child: const Text('OK'),
            ),
          ],
        ),
      ),
    );

    if (result != null && mounted) {
      setState(() {
        _selectedEngineerModalityIds
          ..clear()
          ..addAll(result);
      });
    }
  }

  Future<void> _showManufacturerPicker() async {
    final selectedIds = Set<int>.from(_selectedEngineerManufactureIds);
    final result = await showDialog<Set<int>>(
      context: context,
      builder: (dialogContext) => StatefulBuilder(
        builder: (context, setDialogState) => AlertDialog(
          title: const Text('Select Manufacturer'),
          content: SizedBox(
            width: 360,
            height: 280,
            child: ListView(
              children: _manufactures
                  .map(
                    (item) => CheckboxListTile(
                      contentPadding: EdgeInsets.zero,
                      title: Text(item.name),
                      value: selectedIds.contains(item.id),
                      onChanged: (checked) => setDialogState(() {
                        if (checked == true) {
                          selectedIds.add(item.id);
                        } else {
                          selectedIds.remove(item.id);
                        }
                      }),
                    ),
                  )
                  .toList(),
            ),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(dialogContext).pop(),
              child: const Text('CANCEL'),
            ),
            TextButton(
              onPressed: () => Navigator.of(dialogContext).pop(selectedIds),
              child: const Text('OK'),
            ),
          ],
        ),
      ),
    );

    if (result != null && mounted) {
      setState(() {
        _selectedEngineerManufactureIds
          ..clear()
          ..addAll(result);
      });
    }
  }

  Future<Set<int>?> _showSearchableMultiSelect<T>({
    required String title,
    required List<T> items,
    required Set<int> selectedIds,
    required int Function(T) idOf,
    required String Function(T) nameOf,
  }) async {
    final searchController = TextEditingController();
    var query = '';
    final result = await showDialog<Set<int>>(
      context: context,
      builder: (dialogContext) {
        final dialogSelectedIds = Set<int>.from(selectedIds);
        return StatefulBuilder(
          builder: (context, setDialogState) {
            final filteredItems = items
                .where(
                  (item) =>
                      nameOf(item).toLowerCase().contains(query.toLowerCase()),
                )
                .toList();
            return AlertDialog(
              title: Text(title),
              content: SizedBox(
                width: 400,
                height: MediaQuery.sizeOf(context).height * 0.55,
                child: Column(
                  children: [
                    TextField(
                      controller: searchController,
                      decoration: const InputDecoration(
                        labelText: 'Search',
                        prefixIcon: Icon(Icons.search),
                      ),
                      onChanged: (value) =>
                          setDialogState(() => query = value.trim()),
                    ),
                    const SizedBox(height: 8),
                    Expanded(
                      child: filteredItems.isEmpty
                          ? const Center(child: Text('No matching options.'))
                          : ListView(
                              children: filteredItems.map((item) {
                                final id = idOf(item);
                                return CheckboxListTile(
                                  contentPadding: EdgeInsets.zero,
                                  title: Text(nameOf(item)),
                                  value: dialogSelectedIds.contains(id),
                                  onChanged: (checked) => setDialogState(() {
                                    if (checked == true) {
                                      dialogSelectedIds.add(id);
                                    } else {
                                      dialogSelectedIds.remove(id);
                                    }
                                  }),
                                );
                              }).toList(),
                            ),
                    ),
                  ],
                ),
              ),
              actions: [
                TextButton(
                  onPressed: () => Navigator.of(dialogContext).pop(),
                  child: const Text('CANCEL'),
                ),
                FilledButton(
                  onPressed: () =>
                      Navigator.of(dialogContext).pop(dialogSelectedIds),
                  child: const Text('DONE'),
                ),
              ],
            );
          },
        );
      },
    );
    searchController.dispose();
    return result;
  }

  Future<void> _selectEngineerStates() async {
    final selection = await _showSearchableMultiSelect<StateOption>(
      title: 'Select States',
      items: _states,
      selectedIds: _selectedEngineerStateIds,
      idOf: (state) => state.id,
      nameOf: (state) => state.name,
    );
    if (selection == null || !mounted) return;

    await _loadCitiesForStates(selection);
  }

  Future<void> _loadCitiesForStates(Set<int> stateIds) async {
    final selectedStateIds = Set<int>.from(stateIds);
    final requestId = ++_cityRequestId;
    final stillSelectedCities = _cities
        .where((city) => selectedStateIds.contains(city.stateId))
        .map((city) => city.id)
        .where(_selectedEngineerCityIds.contains)
        .toSet();
    final stillRelevantCities = _cities
        .where((city) => selectedStateIds.contains(city.stateId))
        .toList();

    setState(() {
      _selectedEngineerStateIds
        ..clear()
        ..addAll(selectedStateIds);
      _selectedEngineerCityIds
        ..clear()
        ..addAll(stillSelectedCities);
      _cities = stillRelevantCities;
      _isLoadingCities = selectedStateIds.isNotEmpty;
      _error = null;
    });

    if (selectedStateIds.isEmpty) return;

    try {
      final cities = await _api.getCitiesForStates(selectedStateIds.toList());
      if (!mounted || requestId != _cityRequestId) return;
      final sortedCities = cities.where((city) => city.isEnabled).toList()
        ..sort((a, b) {
          final byName = a.name.toLowerCase().compareTo(b.name.toLowerCase());
          return byName != 0
              ? byName
              : a.stateName.toLowerCase().compareTo(b.stateName.toLowerCase());
        });
      final validCityIds = sortedCities.map((city) => city.id).toSet();
      setState(() {
        _cities = sortedCities;
        _selectedEngineerCityIds.retainAll(validCityIds);
        _isLoadingCities = false;
      });
    } on ApiException catch (error) {
      if (mounted && requestId == _cityRequestId) {
        setState(() {
          _isLoadingCities = false;
          _error = 'Could not load cities: ${error.message}';
        });
      }
    } catch (error) {
      if (mounted && requestId == _cityRequestId) {
        setState(() {
          _isLoadingCities = false;
          _error = 'Could not load cities: $error';
        });
      }
    }
  }

  Future<void> _selectEngineerCities() async {
    if (_selectedEngineerStateIds.isEmpty || _isLoadingCities) return;
    final selection = await _showSearchableMultiSelect<CityOption>(
      title: 'Select Cities',
      items: _cities,
      selectedIds: _selectedEngineerCityIds,
      idOf: (city) => city.id,
      nameOf: (city) => '${city.name}, ${city.stateName}',
    );
    if (selection == null || !mounted) return;
    setState(() {
      _selectedEngineerCityIds
        ..clear()
        ..addAll(selection);
      _error = null;
    });
  }

  Widget _buildLocationMultiSelect({
    required String label,
    required String placeholder,
    required bool enabled,
    required bool loading,
    required List<String> selectedNames,
    required VoidCallback onTap,
    required ValueChanged<int> onRemove,
  }) {
    return InkWell(
      onTap: enabled ? onTap : null,
      borderRadius: BorderRadius.circular(8),
      child: InputDecorator(
        isEmpty: false,
        decoration: InputDecoration(
          labelText: label,
          enabled: enabled,
          border: const OutlineInputBorder(),
          suffixIcon: loading
              ? const Padding(
                  padding: EdgeInsets.all(12),
                  child: SizedBox(
                    width: 18,
                    height: 18,
                    child: CircularProgressIndicator(strokeWidth: 2),
                  ),
                )
              : const Icon(Icons.arrow_drop_down),
        ),
        child: selectedNames.isEmpty
            ? Text(
                placeholder,
                style: TextStyle(color: Theme.of(context).hintColor),
              )
            : Wrap(
                spacing: 6,
                runSpacing: 4,
                children: [
                  for (var index = 0; index < selectedNames.length; index++)
                    InputChip(
                      label: Text(selectedNames[index]),
                      onDeleted: enabled ? () => onRemove(index) : null,
                    ),
                ],
              ),
      ),
    );
  }

  Future<void> _pickEngineerDocuments() async {
    final result = await FilePicker.platform.pickFiles(
      allowMultiple: true,
      withData: true,
      type: FileType.custom,
      allowedExtensions: ['pdf', 'jpg', 'jpeg', 'png'],
    );

    if (result == null || result.files.isEmpty) return;

    final documentType = _documentTypeController.text.trim();
    if (documentType.isEmpty) {
      setState(() => _error = 'Select a document type before uploading files.');
      return;
    }

    if (result.files.any((file) => file.bytes == null || file.bytes!.isEmpty)) {
      setState(() => _error = 'Could not read one or more selected documents.');
      return;
    }

    final selectedBytes = result.files.fold<int>(
      0,
      (total, file) => total + file.bytes!.length,
    );
    final existingBytes = _documents.fold<int>(
      0,
      (total, document) => total + document.fileSizeBytes,
    );
    if (result.files.any(
      (file) => file.bytes!.length > _maxDocumentSizeBytes,
    )) {
      setState(() => _error = 'Each document must be 10 MB or smaller.');
      return;
    }
    if (existingBytes + selectedBytes > _maxTotalDocumentSizeBytes) {
      setState(
        () => _error = 'The total size of documents must be 20 MB or smaller.',
      );
      return;
    }

    setState(() {
      for (final file in result.files) {
        _documents.add(
          _EngineerDocumentItem(
            documentType: documentType,
            originalFileName: file.name,
            contentType: switch (file.extension?.toLowerCase()) {
              'pdf' => 'application/pdf',
              'jpg' || 'jpeg' => 'image/jpeg',
              'png' => 'image/png',
              _ => 'application/octet-stream',
            },
            fileSizeBytes: file.bytes!.length,
            fileContentBase64: base64Encode(file.bytes!),
          ),
        );
      }
      _error = null;
    });
  }

  Future<void> _register() async {
    if (!_formKey.currentState!.validate()) return;

    final engineerValidationError = _role == UserRole.engineer
        ? _validateEngineerForm()
        : null;
    if (engineerValidationError != null) {
      setState(() => _error = engineerValidationError);
      return;
    }

    setState(() {
      _isLoading = true;
      _error = null;
    });
    final now = DateTime.now().toUtc().toIso8601String();

    try {
      if (_role == UserRole.customer) {
        await _api.registerCustomer({
          'customerEquipmentRegistrationId': 0,
          'facilityName': _controller('facilityName').text.trim(),
          'customerContactName': _controller('customerContactName').text.trim(),
          'customerContactMobile': _controller('customerContactMobile').text
              .trim(),
          'primaryContactEmail': _controller('primaryContactEmail').text.trim(),
          'password': _controller('password').text,
          'facilityAddress': _controller('facilityAddress').text.trim(),
          'equipmentModalityId': _selectedEquipmentModalityId,
          'manufactureId': _selectedManufactureId,
          'modelIdentifier': _controller('modelIdentifier').text.trim(),
          'equipmentSerialNumber': _controller('equipmentSerialNumber').text
              .trim(),
          'softwareFirmwareVersion':
              _controller('softwareFirmwareVersion').text.trim().isEmpty
              ? null
              : _controller('softwareFirmwareVersion').text.trim(),
          'coverageStatusId': _selectedCoverageStatusId,
          'isEnabled': true,
          'createdDateTime': now,
        });
      } else {
        await _api.registerEngineer({
          'engineerName': _controller('engineerName').text.trim(),
          'baseLocation': _controller('baseLocation').text.trim(),
          'mobileNumber': _controller('mobileNumber').text.trim(),
          'emailAddress': _controller('emailAddress').text.trim(),
          'password': _controller('password').text,
          'employmentStatus': _selectedEmploymentStatus,
          'capabilities': [
            for (final modalityId in _selectedEngineerModalityIds)
              for (final manufactureId in _selectedEngineerManufactureIds)
                {
                  'equipmentModalityId': modalityId,
                  'manufactureId': manufactureId,
                },
          ],
          'operatingStates': _selectedEngineerStateIds
              .map((stateId) => {'stateId': stateId})
              .toList(),
          'operatingCities': _selectedEngineerCityIds
              .map((cityId) => {'cityId': cityId})
              .toList(),
          'documents': _documents
              .map(
                (item) => {
                  'documentType': item.documentType,
                  'originalFileName': item.originalFileName,
                  'storedFilePath': '',
                  'contentType': item.contentType,
                  'fileSizeBytes': item.fileSizeBytes,
                  'fileContentBase64': item.fileContentBase64,
                },
              )
              .toList(),
          'isEnabled': true,
          'createdDateTime': now,
        });
      }

      if (mounted) {
        await showDialog<void>(
          context: context,
          barrierDismissible: false,
          builder: (context) => AlertDialog(
            title: const Text('Registration successful'),
            content: const Text('Your account has been created successfully.'),
            actions: [
              TextButton(
                onPressed: () => Navigator.of(context).pop(),
                child: const Text('OK'),
              ),
            ],
          ),
        );
      }

      if (mounted) {
        Navigator.of(context).pushAndRemoveUntil(
          MaterialPageRoute(builder: (_) => const LoginScreen()),
          (_) => false,
        );
      }
    } on ApiException catch (error) {
      if (mounted) {
        setState(() => _error = error.message);
      }
    } catch (error) {
      if (mounted) {
        setState(() => _error = 'Could not connect to the API: $error');
      }
    } finally {
      if (mounted) setState(() => _isLoading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final fields = _role == UserRole.customer
        ? _customerFields
        : _engineerFields;
    return Scaffold(
      appBar: AppBar(title: const Text('Create Account')),
      body: _isLoading || _isLoadingReferenceData
          ? LoadingIndicator(
              message: _isLoading
                  ? 'Sending registration...'
                  : 'Loading form data...',
            )
          : SafeArea(
              child: Form(
                key: _formKey,
                child: ListView(
                  padding: const EdgeInsets.fromLTRB(20, 8, 20, 28),
                  children: [
                    const Text(
                      'Choose account type',
                      style: TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                    RadioGroup<UserRole>(
                      groupValue: _role,
                      onChanged: (value) {
                        if (value != null) {
                          setState(() {
                            _role = value;
                            _error = null;
                          });
                        }
                      },
                      child: Wrap(
                        spacing: 12,
                        children: UserRole.values
                            .map(
                              (role) => Row(
                                mainAxisSize: MainAxisSize.min,
                                children: [
                                  Radio<UserRole>(value: role),
                                  Text(role.label),
                                ],
                              ),
                            )
                            .toList(),
                      ),
                    ),
                    ...fields.entries.map((entry) {
                      if (entry.key == 'equipmentModalityId') {
                        return Padding(
                          padding: const EdgeInsets.only(bottom: 13),
                          child: DropdownButtonFormField<int>(
                            initialValue: _selectedEquipmentModalityId,
                            decoration: const InputDecoration(
                              labelText: 'Equipment Modality',
                            ),
                            items: _equipmentModalities
                                .map(
                                  (modality) => DropdownMenuItem<int>(
                                    value: modality.id,
                                    child: Text(modality.name),
                                  ),
                                )
                                .toList(),
                            onChanged: (value) => setState(
                              () => _selectedEquipmentModalityId = value,
                            ),
                            validator: (value) => value == null
                                ? 'Select an equipment modality.'
                                : null,
                          ),
                        );
                      }

                      if (entry.key == 'manufactureId') {
                        return Padding(
                          padding: const EdgeInsets.only(bottom: 13),
                          child: DropdownButtonFormField<int>(
                            initialValue: _selectedManufactureId,
                            decoration: const InputDecoration(
                              labelText: 'Manufacture',
                            ),
                            items: _manufactures
                                .map(
                                  (manufacture) => DropdownMenuItem<int>(
                                    value: manufacture.id,
                                    child: Text(manufacture.name),
                                  ),
                                )
                                .toList(),
                            onChanged: (value) =>
                                setState(() => _selectedManufactureId = value),
                            validator: (value) =>
                                value == null ? 'Select a manufacture.' : null,
                          ),
                        );
                      }

                      if (entry.key == 'coverageStatusId') {
                        return Padding(
                          padding: const EdgeInsets.only(bottom: 13),
                          child: DropdownButtonFormField<int>(
                            initialValue: _selectedCoverageStatusId,
                            decoration: const InputDecoration(
                              labelText: 'Coverage Status',
                            ),
                            items: _coverageStatuses
                                .map(
                                  (status) => DropdownMenuItem<int>(
                                    value: status.id,
                                    child: Text(status.name),
                                  ),
                                )
                                .toList(),
                            onChanged: (value) => setState(
                              () => _selectedCoverageStatusId = value,
                            ),
                            validator: (value) => value == null
                                ? 'Select a coverage status.'
                                : null,
                          ),
                        );
                      }

                      return Padding(
                        padding: const EdgeInsets.only(bottom: 13),
                        child: AppTextField(
                          controller: _controller(entry.key),
                          label: entry.value,
                          keyboardType:
                              entry.key == 'emailAddress' ||
                                  entry.key == 'primaryContactEmail'
                              ? TextInputType.emailAddress
                              : entry.key == 'customerContactMobile' ||
                                    entry.key == 'mobileNumber'
                              ? TextInputType.phone
                              : TextInputType.text,
                          obscureText:
                              entry.key == 'password' ||
                              entry.key == 'confirmPassword',
                          validator: (value) => _validate(entry.key, value),
                        ),
                      );
                    }),
                    if (_role == UserRole.engineer) ...[
                      // MODIFIED: Engineer-specific fields now match the backend entity.
                      const SizedBox(height: 8),
                      Card(
                        child: Padding(
                          padding: const EdgeInsets.all(12),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              const Text(
                                'Employment Status',
                                style: TextStyle(
                                  fontSize: 14,
                                  fontWeight: FontWeight.w600,
                                ),
                              ),
                              const SizedBox(height: 8),
                              FormField<String>(
                                initialValue: _selectedEmploymentStatus,
                                validator: (value) => value == null
                                    ? 'Select an employment status.'
                                    : null,
                                builder: (field) => InputDecorator(
                                  decoration: InputDecoration(
                                    labelText: 'Select status',
                                    errorText: field.errorText,
                                    border: InputBorder.none,
                                  ),
                                  child: RadioGroup<String>(
                                    groupValue: _selectedEmploymentStatus,
                                    onChanged: (value) {
                                      setState(
                                        () => _selectedEmploymentStatus = value,
                                      );
                                      field.didChange(value);
                                    },
                                    child: const Wrap(
                                      spacing: 12,
                                      children: [
                                        Row(
                                          mainAxisSize: MainAxisSize.min,
                                          children: [
                                            Radio<String>(value: 'Full-Time'),
                                            Text('Full-Time'),
                                          ],
                                        ),
                                        Row(
                                          mainAxisSize: MainAxisSize.min,
                                          children: [
                                            Radio<String>(value: 'Part-Time'),
                                            Text('Part-Time'),
                                          ],
                                        ),
                                        Row(
                                          mainAxisSize: MainAxisSize.min,
                                          children: [
                                            Radio<String>(value: 'Freelancer'),
                                            Text('Freelancer'),
                                          ],
                                        ),
                                      ],
                                    ),
                                  ),
                                ),
                              ),
                              const SizedBox(height: 16),
                              const Text(
                                'Modality / Manufacturer Working With',
                                style: TextStyle(
                                  fontSize: 14,
                                  fontWeight: FontWeight.w600,
                                ),
                              ),
                              const SizedBox(height: 8),
                              InkWell(
                                onTap: _showEquipmentModalityPicker,
                                child: InputDecorator(
                                  decoration: const InputDecoration(
                                    labelText: 'Equipment Modality',
                                    border: OutlineInputBorder(),
                                    suffixIcon: Icon(Icons.arrow_drop_down),
                                  ),
                                  child: Text(
                                    _selectedEngineerModalityIds.isEmpty
                                        ? 'Select equipment modalities'
                                        : _equipmentModalities
                                              .where(
                                                (item) =>
                                                    _selectedEngineerModalityIds
                                                        .contains(item.id),
                                              )
                                              .map((item) => item.name)
                                              .join(', '),
                                    maxLines: 2,
                                    overflow: TextOverflow.ellipsis,
                                  ),
                                ),
                              ),
                              const Divider(),
                              InkWell(
                                onTap: _showManufacturerPicker,
                                child: InputDecorator(
                                  decoration: const InputDecoration(
                                    labelText: 'Manufacturer',
                                    border: OutlineInputBorder(),
                                    suffixIcon: Icon(Icons.arrow_drop_down),
                                  ),
                                  child: Text(
                                    _selectedEngineerManufactureIds.isEmpty
                                        ? 'Select manufacturers'
                                        : _manufactures
                                              .where(
                                                (item) =>
                                                    _selectedEngineerManufactureIds
                                                        .contains(item.id),
                                              )
                                              .map((item) => item.name)
                                              .join(', '),
                                    maxLines: 2,
                                    overflow: TextOverflow.ellipsis,
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                      const SizedBox(height: 12),
                      Card(
                        child: Padding(
                          padding: const EdgeInsets.all(12),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              const Text(
                                'Training Certificate / Experience Certificate',
                                style: TextStyle(
                                  fontSize: 14,
                                  fontWeight: FontWeight.w600,
                                ),
                              ),
                              const SizedBox(height: 8),
                              DropdownButtonFormField<String>(
                                initialValue: _documentTypeController.text,
                                decoration: const InputDecoration(
                                  labelText: 'Document Type',
                                ),
                                items: const [
                                  DropdownMenuItem(
                                    value: 'Training',
                                    child: Text('Training Certificate'),
                                  ),
                                  DropdownMenuItem(
                                    value: 'Experience',
                                    child: Text('Experience Certificate'),
                                  ),
                                ],
                                onChanged: (value) {
                                  if (value != null) {
                                    _documentTypeController.text = value;
                                  }
                                },
                              ),
                              const SizedBox(height: 8),
                              Align(
                                alignment: Alignment.centerLeft,
                                child: FilledButton.icon(
                                  onPressed: _pickEngineerDocuments,
                                  icon: const Icon(Icons.upload_file),
                                  label: const Text('Upload Files'),
                                ),
                              ),
                              if (_documents.isNotEmpty) ...[
                                const SizedBox(height: 12),
                                ListView.separated(
                                  shrinkWrap: true,
                                  physics: const NeverScrollableScrollPhysics(),
                                  itemCount: _documents.length,
                                  separatorBuilder: (_, _) =>
                                      const SizedBox(height: 8),
                                  itemBuilder: (context, index) {
                                    final item = _documents[index];
                                    return Container(
                                      padding: const EdgeInsets.all(10),
                                      decoration: BoxDecoration(
                                        border: Border.all(
                                          color: Colors.grey.shade300,
                                        ),
                                        borderRadius: BorderRadius.circular(8),
                                      ),
                                      child: Row(
                                        children: [
                                          Expanded(
                                            child: Column(
                                              crossAxisAlignment:
                                                  CrossAxisAlignment.start,
                                              children: [
                                                Text(
                                                  item.documentType ==
                                                          'Training'
                                                      ? 'Training Certificate'
                                                      : 'Experience Certificate',
                                                  style: const TextStyle(
                                                    fontWeight: FontWeight.w500,
                                                  ),
                                                ),
                                                Text(item.originalFileName),
                                              ],
                                            ),
                                          ),
                                          IconButton(
                                            tooltip: 'Remove file',
                                            onPressed: () => setState(
                                              () => _documents.removeAt(index),
                                            ),
                                            icon: const Icon(
                                              Icons.delete_outline,
                                            ),
                                          ),
                                        ],
                                      ),
                                    );
                                  },
                                ),
                              ],
                            ],
                          ),
                        ),
                      ),
                      const SizedBox(height: 12),
                      Card(
                        child: Padding(
                          padding: const EdgeInsets.all(12),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              const Text(
                                'Geographic Operating Radius',
                                style: TextStyle(
                                  fontSize: 14,
                                  fontWeight: FontWeight.w600,
                                ),
                              ),
                              const SizedBox(height: 8),
                              _buildLocationMultiSelect(
                                label: 'States',
                                placeholder: 'Select states',
                                enabled: true,
                                loading: false,
                                selectedNames: _states
                                    .where(
                                      (state) => _selectedEngineerStateIds
                                          .contains(state.id),
                                    )
                                    .map((state) => state.name)
                                    .toList(),
                                onTap: _selectEngineerStates,
                                onRemove: (index) {
                                  final selected = _states
                                      .where(
                                        (state) => _selectedEngineerStateIds
                                            .contains(state.id),
                                      )
                                      .toList();
                                  final nextIds = Set<int>.from(
                                    _selectedEngineerStateIds,
                                  )..remove(selected[index].id);
                                  _loadCitiesForStates(nextIds);
                                },
                              ),
                              const SizedBox(height: 12),
                              _buildLocationMultiSelect(
                                label: 'Cities',
                                placeholder: _selectedEngineerStateIds.isEmpty
                                    ? 'Select a state first'
                                    : 'Select cities',
                                enabled:
                                    _selectedEngineerStateIds.isNotEmpty &&
                                    !_isLoadingCities,
                                loading: _isLoadingCities,
                                selectedNames: _cities
                                    .where(
                                      (city) => _selectedEngineerCityIds
                                          .contains(city.id),
                                    )
                                    .map(
                                      (city) =>
                                          '${city.name}, ${city.stateName}',
                                    )
                                    .toList(),
                                onTap: _selectEngineerCities,
                                onRemove: (index) {
                                  final selected = _cities
                                      .where(
                                        (city) => _selectedEngineerCityIds
                                            .contains(city.id),
                                      )
                                      .toList();
                                  setState(
                                    () => _selectedEngineerCityIds.remove(
                                      selected[index].id,
                                    ),
                                  );
                                },
                              ),
                            ],
                          ),
                        ),
                      ),
                    ],
                    Container(
                      padding: const EdgeInsets.all(12),
                      decoration: BoxDecoration(
                        color: const Color(0xFFE7F1EF),
                        borderRadius: BorderRadius.circular(8),
                      ),
                      child: Text(
                        _role == UserRole.customer
                            ? 'Enter the equipment details required for this customer registration.'
                            : 'Enter the engineer employment, modality, training, and operating location details required by the API.',
                        style: const TextStyle(fontSize: 13),
                      ),
                    ),
                    if (_error != null) ...[
                      const SizedBox(height: 12),
                      Text(
                        _error!,
                        style: TextStyle(
                          color: _error == 'Registration completed.'
                              ? Colors.green.shade800
                              : Colors.red,
                        ),
                      ),
                    ],
                    const SizedBox(height: 16),
                    FilledButton(
                      onPressed: _register,
                      child: const Text('Create Account'),
                    ),
                  ],
                ),
              ),
            ),
    );
  }
}

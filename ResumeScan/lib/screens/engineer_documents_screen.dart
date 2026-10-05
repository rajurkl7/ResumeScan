import 'package:flutter/material.dart';

import '../models/engineer_document.dart';
import '../services/api_service.dart';
import '../utils/app_theme.dart';
import '../widgets/loading_indicator.dart';

class EngineerDocumentsScreen extends StatefulWidget {
  const EngineerDocumentsScreen({
    super.key,
    required this.fieldServiceEngineerId,
  });

  final int fieldServiceEngineerId;

  @override
  State<EngineerDocumentsScreen> createState() =>
      _EngineerDocumentsScreenState();
}

class _EngineerDocumentsScreenState extends State<EngineerDocumentsScreen> {
  final ApiService _api = ApiService();
  late Future<List<EngineerDocument>> _documentsFuture;
  int? _downloadingDocumentId;

  @override
  void initState() {
    super.initState();
    _documentsFuture = _api.getEngineerDocuments(widget.fieldServiceEngineerId);
  }

  Future<void> _download(EngineerDocument document) async {
    setState(() => _downloadingDocumentId = document.id);
    try {
      final saved = await _api.downloadEngineerDocument(
        fieldServiceEngineerId: widget.fieldServiceEngineerId,
        document: document,
      );
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(
              saved
                  ? '${document.originalFileName} download started.'
                  : 'Download cancelled.',
            ),
          ),
        );
      }
    } on ApiException catch (error) {
      _showError(error.message);
    } catch (error) {
      _showError('Could not download the attachment: $error');
    } finally {
      if (mounted) setState(() => _downloadingDocumentId = null);
    }
  }

  void _showError(String message) {
    if (!mounted) return;
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(content: Text(message), backgroundColor: Colors.red.shade700),
    );
  }

  String _formatSize(int? size) {
    if (size == null) return 'Size unavailable';
    if (size < 1024 * 1024) return '${(size / 1024).ceil()} KB';
    return '${(size / (1024 * 1024)).toStringAsFixed(1)} MB';
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('My Documents')),
      body: FutureBuilder<List<EngineerDocument>>(
        future: _documentsFuture,
        builder: (context, snapshot) {
          if (snapshot.connectionState != ConnectionState.done) {
            return const LoadingIndicator(message: 'Loading attachments...');
          }
          if (snapshot.hasError) {
            return Center(
              child: Padding(
                padding: const EdgeInsets.all(24),
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Text(
                      'Could not load attachments: ${snapshot.error}',
                      textAlign: TextAlign.center,
                    ),
                    const SizedBox(height: 12),
                    OutlinedButton.icon(
                      onPressed: () {
                        setState(() {
                          _documentsFuture = _api.getEngineerDocuments(
                            widget.fieldServiceEngineerId,
                          );
                        });
                      },
                      icon: const Icon(Icons.refresh),
                      label: const Text('Try again'),
                    ),
                  ],
                ),
              ),
            );
          }

          final documents = snapshot.data ?? const <EngineerDocument>[];
          if (documents.isEmpty) {
            return const Center(child: Text('No attachments found.'));
          }

          return ListView.separated(
            padding: const EdgeInsets.all(16),
            itemCount: documents.length,
            separatorBuilder: (_, _) => const SizedBox(height: 8),
            itemBuilder: (context, index) {
              final document = documents[index];
              final isDownloading = _downloadingDocumentId == document.id;
              return Card(
                child: ListTile(
                  leading: const Icon(
                    Icons.insert_drive_file_outlined,
                    color: AppTheme.teal,
                  ),
                  title: Text(document.originalFileName),
                  subtitle: Text(
                    '${document.documentType} · ${_formatSize(document.fileSizeBytes)}',
                  ),
                  trailing: isDownloading
                      ? const SizedBox(
                          width: 24,
                          height: 24,
                          child: CircularProgressIndicator(strokeWidth: 2),
                        )
                      : IconButton(
                          tooltip: 'Download ${document.originalFileName}',
                          onPressed: _downloadingDocumentId == null
                              ? () => _download(document)
                              : null,
                          icon: const Icon(Icons.download_outlined),
                        ),
                ),
              );
            },
          );
        },
      ),
    );
  }
}

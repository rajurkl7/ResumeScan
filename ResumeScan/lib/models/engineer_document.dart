class EngineerDocument {
  const EngineerDocument({
    required this.id,
    required this.documentType,
    required this.originalFileName,
    required this.contentType,
    required this.fileSizeBytes,
    required this.uploadedDateTime,
  });

  final int id;
  final String documentType;
  final String originalFileName;
  final String? contentType;
  final int? fileSizeBytes;
  final DateTime? uploadedDateTime;

  factory EngineerDocument.fromJson(Map<String, dynamic> json) {
    return EngineerDocument(
      id: (json['fieldServiceEngineerDocumentId'] ??
              json['FieldServiceEngineerDocumentId'] ??
              0)
          as int,
      documentType:
          '${json['documentType'] ?? json['DocumentType'] ?? ''}',
      originalFileName:
          '${json['originalFileName'] ?? json['OriginalFileName'] ?? ''}',
      contentType:
          json['contentType'] as String? ?? json['ContentType'] as String?,
      fileSizeBytes:
          json['fileSizeBytes'] as int? ?? json['FileSizeBytes'] as int?,
      uploadedDateTime: DateTime.tryParse(
        '${json['uploadedDateTime'] ?? json['UploadedDateTime'] ?? ''}',
      ),
    );
  }
}

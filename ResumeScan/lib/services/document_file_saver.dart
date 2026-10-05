import 'dart:typed_data';

import 'document_file_saver_stub.dart'
    if (dart.library.html) 'document_file_saver_web.dart' as platform_saver;

Future<bool> saveDocumentBytes({
  required String fileName,
  required String contentType,
  required Uint8List bytes,
}) {
  return platform_saver.saveDocumentBytes(
    fileName: fileName,
    contentType: contentType,
    bytes: bytes,
  );
}

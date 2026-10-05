import 'dart:typed_data';

import 'package:file_picker/file_picker.dart';

Future<bool> saveDocumentBytes({
  required String fileName,
  required String contentType,
  required Uint8List bytes,
}) async {
  final savedPath = await FilePicker.platform.saveFile(
    dialogTitle: 'Save attachment',
    fileName: fileName,
    bytes: bytes,
  );
  return savedPath != null;
}

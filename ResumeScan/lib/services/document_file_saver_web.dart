import 'dart:async';
import 'dart:js_interop';
import 'dart:typed_data';

import 'package:web/web.dart' as web;

Future<bool> saveDocumentBytes({
  required String fileName,
  required String contentType,
  required Uint8List bytes,
}) async {
  final blob = web.Blob(
    [bytes.toJS].toJS,
    web.BlobPropertyBag(type: contentType),
  );
  final objectUrl = web.URL.createObjectURL(blob);
  final anchor = web.HTMLAnchorElement()
    ..href = objectUrl
    ..download = fileName
    ..style.display = 'none';

  try {
    web.document.body!.append(anchor);
    anchor.click();
  } finally {
    anchor.remove();
    Timer(const Duration(seconds: 1), () => web.URL.revokeObjectURL(objectUrl));
  }
  return true;
}

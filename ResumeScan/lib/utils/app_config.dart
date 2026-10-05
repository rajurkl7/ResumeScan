import 'package:flutter/foundation.dart';

class AppConfig {
  // Android emulators reach the development computer through 10.0.2.2.
  // iOS simulators can use localhost. For a phone, use your computer's LAN IP.
  static final String apiBaseUrl = !kIsWeb && defaultTargetPlatform == TargetPlatform.android
      ? 'https://10.0.2.2:7092'
      : 'https://localhost:7092';

  // Set these to the BasicAuthentication credentials configured by the API.
  // Do not put production secrets in a mobile app; use a proper user auth flow.
  static const String apiUsername = 'admin';
  static const String apiPassword = 'Pass@998877';
}
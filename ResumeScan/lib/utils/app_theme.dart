import 'package:flutter/material.dart';

class AppTheme {
  static const Color ink = Color(0xFF172B3A);
  static const Color teal = Color(0xFF167D78);
  static const Color canvas = Color(0xFFF4F7F6);

  static ThemeData get light => ThemeData(
        useMaterial3: true,
        scaffoldBackgroundColor: canvas,
        colorScheme: ColorScheme.fromSeed(seedColor: teal, primary: teal),
        appBarTheme: const AppBarTheme(backgroundColor: canvas),
      );
}

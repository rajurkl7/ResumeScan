import 'package:flutter/material.dart';

import 'screens/splash_screen.dart';
import 'utils/app_theme.dart';

void main() {
  runApp(const ResumeScanApp());
}

class ResumeScanApp extends StatelessWidget {
  const ResumeScanApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'ResumeScan',
      debugShowCheckedModeBanner: false,
      theme: AppTheme.light,
      home: const SplashScreen(),
    );
  }
}

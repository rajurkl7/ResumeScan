// This is a basic Flutter widget test.
//
// To perform an interaction with a widget in your test, use the WidgetTester
// utility in the flutter_test package. For example, you can send tap and scroll
// gestures. You can also use WidgetTester to find child widgets in the widget
// tree, read text, and verify that the values of widget properties are correct.

import 'package:flutter_test/flutter_test.dart';

import 'package:resume_scan/main.dart';

void main() {
  testWidgets('ResumeScan opens the login screen after the splash', (WidgetTester tester) async {
    await tester.pumpWidget(const ResumeScanApp());
    expect(find.text('ResumeScan'), findsOneWidget);

    await tester.pump(const Duration(milliseconds: 900));
    await tester.pumpAndSettle();
    expect(find.text('Login'), findsOneWidget);
    expect(find.text('Create Account'), findsOneWidget);
  });
}

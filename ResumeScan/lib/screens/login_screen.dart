import 'package:flutter/material.dart';

import '../models/customer_account.dart';
import '../models/engineer_account.dart';
import '../models/user_role.dart';
import '../services/api_service.dart';
import '../widgets/app_text_field.dart';
import '../widgets/loading_indicator.dart';
import 'dashboard_screen.dart';
import 'registration_screen.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _formKey = GlobalKey<FormState>();
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  final _api = ApiService();
  UserRole _role = UserRole.customer;
  bool _isLoading = false;
  String? _error;

  @override
  void dispose() {
    _emailController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  Future<void> _login() async {
    if (!_formKey.currentState!.validate()) return;
    setState(() {
      _isLoading = true;
      _error = null;
    });
    try {
      final email = _emailController.text.trim();
      final user = _role == UserRole.customer
          ? _toCustomerUser(await _api.loginCustomer(email, _passwordController.text))
          : _toEngineerUser(await _api.loginEngineer(email, _passwordController.text));
      if (!mounted) return;
      Navigator.of(context).pushReplacement(
        MaterialPageRoute(builder: (_) => DashboardScreen(user: user)),
      );
    } on ApiException catch (error) {
      if (mounted) setState(() => _error = error.message);
    } catch (error) {
      if (mounted) setState(() => _error = 'Could not connect to the API: $error');
    } finally {
      if (mounted) setState(() => _isLoading = false);
    }
  }

  AppUser _toCustomerUser(CustomerAccount customer) =>
      AppUser(name: customer.name, email: customer.email, role: UserRole.customer);

  AppUser _toEngineerUser(EngineerAccount engineer) =>
      AppUser(
        name: engineer.name,
        email: engineer.email,
        role: UserRole.engineer,
        engineerId: engineer.id,
      );

  String? _required(String? value) =>
      value == null || value.trim().isEmpty ? 'This field is required.' : null;

  String? _validEmail(String? value) {
    if (_required(value) != null) return 'Enter your email address.';
    return RegExp(r'^[^\s@]+@[^\s@]+\.[^\s@]+$').hasMatch(value!.trim())
        ? null
        : 'Enter a valid email address.';
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: Center(
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(24),
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 440),
              child: _isLoading
                  ? const LoadingIndicator(message: 'Checking your account...')
                  : Form(
                      key: _formKey,
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.stretch,
                        children: [
                          const Icon(Icons.document_scanner_outlined, size: 48, color: Color(0xFF167D78)),
                          const SizedBox(height: 14),
                          Text(
                            'ResumeScan',
                            textAlign: TextAlign.center,
                            style: Theme.of(context).textTheme.headlineMedium?.copyWith(fontWeight: FontWeight.w700),
                          ),
                          const SizedBox(height: 8),
                          const Text('Sign in to your service workspace', textAlign: TextAlign.center),
                          const SizedBox(height: 28),
                          AppTextField(
                            controller: _emailController,
                            label: 'Email ID',
                            keyboardType: TextInputType.emailAddress,
                            validator: _validEmail,
                          ),
                          const SizedBox(height: 14),
                          AppTextField(
                            controller: _passwordController,
                            label: 'Password',
                            obscureText: true,
                            validator: (value) {
                              final required = _required(value);
                              if (required != null) return required;
                              return value!.length >= 8 ? null : 'Use at least 8 characters.';
                            },
                          ),
                          const SizedBox(height: 12),
                          RadioGroup<UserRole>(
                            groupValue: _role,
                            onChanged: (value) {
                              if (value != null) setState(() => _role = value);
                            },
                            child: Wrap(
                              spacing: 12,
                              children: UserRole.values.map((role) => Row(
                                mainAxisSize: MainAxisSize.min,
                                children: [
                                  Radio<UserRole>(value: role),
                                  Text(role.label),
                                ],
                              )).toList(),
                            ),
                          ),
                          if (_error != null) ...[
                            const SizedBox(height: 8),
                            Text(_error!, style: const TextStyle(color: Colors.red)),
                          ],
                          const SizedBox(height: 14),
                          FilledButton(onPressed: _login, child: const Text('Login')),
                          Align(
                            alignment: Alignment.center,
                            child: TextButton(
                              onPressed: () => _showMessage(
                                'Password reset is not available because the API has no password-reset route.',
                              ),
                              child: const Text('Forgot Password?'),
                            ),
                          ),
                          OutlinedButton.icon(
                            onPressed: () => Navigator.of(context).push(
                              MaterialPageRoute(builder: (_) => const RegistrationScreen()),
                            ),
                            icon: const Icon(Icons.person_add_alt_1),
                            label: const Text('Create Account'),
                          ),
                          const SizedBox(height: 16),
                        ],
                      ),
                    ),
            ),
          ),
        ),
      ),
    );
  }

  void _showMessage(String message) {
    showDialog<void>(
      context: context,
      builder: (context) => AlertDialog(
        content: Text(message),
        actions: [TextButton(onPressed: () => Navigator.pop(context), child: const Text('Close'))],
      ),
    );
  }
}
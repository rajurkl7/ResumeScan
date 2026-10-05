import 'package:flutter/material.dart';

import '../models/user_role.dart';
import '../utils/app_theme.dart';
import 'engineer_documents_screen.dart';
import 'login_screen.dart';

class DashboardScreen extends StatelessWidget {
  const DashboardScreen({super.key, required this.user});

  final AppUser user;

  @override
  Widget build(BuildContext context) {
    final isCustomer = user.role == UserRole.customer;
    final actions = isCustomer
        ? const [
            _DashboardAction('My Equipment', Icons.precision_manufacturing_outlined),
            _DashboardAction('Raise Service Request', Icons.add_task_outlined),
            _DashboardAction('Service History', Icons.history),
            _DashboardAction('Profile', Icons.person_outline),
          ]
        : const [
            _DashboardAction('My Documents', Icons.attach_file_outlined),
            _DashboardAction('Assigned Tickets', Icons.assignment_outlined),
            _DashboardAction('Pending Tickets', Icons.pending_actions_outlined),
            _DashboardAction('Completed Tickets', Icons.task_alt),
            _DashboardAction('Profile', Icons.person_outline),
          ];

    return Scaffold(
      appBar: AppBar(
        title: Text('${user.role.label} Dashboard'),
        actions: [
          IconButton(
            tooltip: 'Logout',
            icon: const Icon(Icons.logout),
            onPressed: () => Navigator.of(context).pushAndRemoveUntil(
              MaterialPageRoute(builder: (_) => const LoginScreen()),
              (route) => false,
            ),
          ),
        ],
      ),
      body: ListView(
        padding: const EdgeInsets.all(20),
        children: [
          Text(
            'Welcome, ${user.name}',
            style: Theme.of(context).textTheme.headlineSmall?.copyWith(
              color: AppTheme.ink,
              fontWeight: FontWeight.w700,
            ),
          ),
          const SizedBox(height: 6),
          Text(user.email, style: const TextStyle(color: Colors.black54)),
          const SizedBox(height: 18),
          Container(
            padding: const EdgeInsets.all(12),
            decoration: BoxDecoration(
              color: const Color(0xFFFFF4DA),
              borderRadius: BorderRadius.circular(8),
            ),
            child: const Text(
              'Demo access: the API confirms this email exists but does not verify the account password. Dashboard data endpoints are not available yet.',
              style: TextStyle(fontSize: 13),
            ),
          ),
          const SizedBox(height: 22),
          ...actions.map((action) => Card(
                margin: const EdgeInsets.only(bottom: 10),
                child: ListTile(
                  leading: Icon(action.icon, color: AppTheme.teal),
                  title: Text(action.label),
                  trailing: const Icon(Icons.chevron_right),
                  onTap: () {
                    if (action.label == 'My Documents' &&
                        user.engineerId != null) {
                      Navigator.of(context).push(
                        MaterialPageRoute<void>(
                          builder: (_) => EngineerDocumentsScreen(
                            fieldServiceEngineerId: user.engineerId!,
                          ),
                        ),
                      );
                      return;
                    }
                    _showNotAvailable(context, action.label);
                  },
                ),
              )),
          TextButton.icon(
            onPressed: () => Navigator.of(context).pushAndRemoveUntil(
              MaterialPageRoute(builder: (_) => const LoginScreen()),
              (route) => false,
            ),
            icon: const Icon(Icons.logout),
            label: const Text('Logout'),
          ),
        ],
      ),
    );
  }

  void _showNotAvailable(BuildContext context, String action) {
    showDialog<void>(
      context: context,
      builder: (context) => AlertDialog(
        title: Text(action),
        content: const Text('This feature needs an API endpoint that is not present in the current backend.'),
        actions: [
          TextButton(onPressed: () => Navigator.pop(context), child: const Text('Close')),
        ],
      ),
    );
  }
}

class _DashboardAction {
  const _DashboardAction(this.label, this.icon);

  final String label;
  final IconData icon;
}
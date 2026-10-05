enum UserRole { customer, engineer }

extension UserRoleLabel on UserRole {
  String get label => this == UserRole.customer ? 'Customer' : 'Engineer';
}

class AppUser {
  const AppUser({
    required this.name,
    required this.email,
    required this.role,
    this.engineerId,
  });

  final String name;
  final String email;
  final UserRole role;
  final int? engineerId;
}
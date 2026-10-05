class CoverageStatus {
  const CoverageStatus({
    required this.id,
    required this.name,
    required this.isEnabled,
  });

  final int id;
  final String name;
  final bool isEnabled;

  factory CoverageStatus.fromJson(Map<String, dynamic> json) {
    return CoverageStatus(
      id: (json['coverageStatusId'] ?? json['CoverageStatusId']) as int,
      name: '${json['coverageStatusName'] ?? json['CoverageStatusName'] ?? ''}',
      isEnabled: (json['isEnabled'] ?? json['IsEnabled'] ?? false) as bool,
    );
  }
}

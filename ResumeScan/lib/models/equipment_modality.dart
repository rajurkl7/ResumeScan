class EquipmentModality {
  const EquipmentModality({
    required this.id,
    required this.name,
    required this.isEnabled,
  });

  final int id;
  final String name;
  final bool isEnabled;

  factory EquipmentModality.fromJson(Map<String, dynamic> json) {
    return EquipmentModality(
      id: (json['equipmentModalityId'] ?? json['EquipmentModalityId']) as int,
      name: '${json['equipmentModalityName'] ?? json['EquipmentModalityName'] ?? ''}',
      isEnabled: (json['isEnabled'] ?? json['IsEnabled'] ?? false) as bool,
    );
  }
}

class Manufacture {
  const Manufacture({
    required this.id,
    required this.name,
    required this.isEnabled,
  });

  final int id;
  final String name;
  final bool isEnabled;

  factory Manufacture.fromJson(Map<String, dynamic> json) {
    return Manufacture(
      id: (json['manufactureId'] ?? json['ManufactureId']) as int,
      name: '${json['manufactureName'] ?? json['ManufactureName'] ?? ''}',
      isEnabled: (json['isEnabled'] ?? json['IsEnabled'] ?? false) as bool,
    );
  }
}

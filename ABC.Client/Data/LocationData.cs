namespace ABC.Client.Data;
public partial class LocationData
{
	public static List<string> Provinces = new List<string> { "Rizal", "Metro Manila", "Manila" };

	public static Dictionary<string, List<string>> Cities = new Dictionary<string, List<string>>
		{
			{ "Rizal", new List<string>
				{
					"Angono",
					"Antipolo",
					"Baras",
					"Binangonan",
					"Cainta",
					"Cardona",
					"Jalajala",
					"Morong",
					"Pililla",
					"Rodriguez",
					"San Mateo",
					"Tanay",
					"Taytay",
					"Teresa"
				}
			},

			{ "Metro Manila", new List<string>
				{
					"City of Manila",
					"Caloocan",
					"Las Piñas",
					"Makati",
					"Malabon",
					"Mandaluyong",
					"Marikina",
					"Muntinlupa",
					"Navotas",
					"Parañaque",
					"Pasay",
					"Pasig",
					"Quezon City",
					"San Juan",
					"Taguig",
					"Valenzuela"
				}
			},

			{ "Manila", new List<string>
				{
					"Binondo",
					"Ermita",
					"Intramuros",
					"Malate",
					"Paco",
					"Pandacan",
					"Port Area",
					"Quiapo",
					"Sampaloc",
					"San Andres",
					"San Miguel",
					"San Nicolas",
					"Santa Ana",
					"Santa Cruz",
					"Santa Mesa",
					"Tondo"
				}
			}
		};

	public static Dictionary<string, decimal> DeliveryFees = new Dictionary<string, decimal>
	{

		// Specific fees for cities within Metro Manila
		{ "Manila", 129.0m },
		{ "Caloocan", 205.0m },
		{ "Las Piñas", 228.0m },
		{ "Makati", 142.0m },
		{ "Malabon", 157.0m },
		{ "Mandaluyong", 113.0m },
		{ "Marikina", 142.0m },
		{ "Muntinlupa", 207.0m },
		{ "Navotas", 201.0m },
		{ "Parañaque", 212.0m },
		{ "Pasay", 157.0m },
		{ "Pasig", 134.0m },
		{ "Quezon City", 139.0m },
		{ "San Juan", 92.0m },
		{ "Taguig", 169.0m },
		{ "Valenzuela", 201.0m },

		// Additional cities within Manila
		{ "Binondo", 151.0m },
		{ "Ermita", 146.0m },
		{ "Intramuros", 157.0m },
		{ "Malate", 142.0m },
		{ "Paco", 140.0m },
		{ "Pandacan", 133.0m },
		{ "Port Area", 160.0m },
		{ "Quiapo", 140.0m },
		{ "Sampaloc", 136.0m },
		{ "San Andres", 147.0m },
		{ "San Miguel", 138.0m },
		{ "San Nicolas", 153.0m },
		{ "Santa Ana", 122.0m },
		{ "Santa Cruz", 122.0m },
		{ "Santa Mesa", 117.0m },
		{ "Tondo", 157.0m },

		// Specific fees for cities within Rizal
		{ "Angono", 215.0m },
		{ "Antipolo", 161.0m },
		{ "Baras", 323.0m },
		{ "Binangonan", 259.0m },
		{ "Cainta", 155.0m },
		{ "Cardona", 290.0m },
		{ "Jalajala", 410.0m },
		{ "Morong", 297.0m },
		{ "Pililla", 344.0m },
		{ "Rodriguez", 243.0m },
		{ "San Mateo", 188.0m },
		{ "Tanay", 368.0m },
		{ "Taytay", 161.0m },
		{ "Teresa", 230.0m }

	};
}


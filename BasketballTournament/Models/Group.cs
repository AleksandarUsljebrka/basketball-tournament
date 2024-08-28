using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballTournament.Models
{
	using System.Collections.Generic;

	public class Group
	{
		public string Name { get; set; }
		public List<BasketballTeam> Teams { get; set; }
	}
}

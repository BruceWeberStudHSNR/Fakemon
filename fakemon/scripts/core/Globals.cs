using Godot;
using System;

namespace  Game.Core
{
	/// <summary>
	/// Verwaltet globale Variablen und bietet Singleton-Zugriff.
	/// </summary>
	public partial class Globals : Node
	{
		public static Globals Instance { get; private set; }

		[ExportCategory("Gameplay")] [Export] 
		public int GRID_SIZE = 16;
		public override void _Ready()
		{
			Instance = this;
			Logger.Info(" Loading Globals...");
		}
	}
}

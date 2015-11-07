using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelPack
{
	#region Fields

	[NonSerialized]
	private List<int> levels;

	#endregion

	#region Constructors

	public LevelPack(int number, string name)
	{
		if (number <= 0)
			throw new ArgumentException("Can not be less or equal to zero.", "number");

		this.LevelPackNumber = number;
		this.Name = name;

		this.IsLocked = false;
	}

	#endregion

	#region Properties

	public string Name { get; set; }

	public int LevelPackNumber { get; set; }

	public bool IsLocked { get; set; }

	public List<int> Levels
	{
		get { return levels; }
		set
		{
			if (value == null)
				throw new UnityException("value is null.");

			levels = value;
		}
	}

	#endregion

	#region Methods

	#endregion
}

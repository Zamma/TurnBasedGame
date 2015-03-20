//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

//credit to Fattie at http://answers.unity3d.com/questions/663351/design-advice-power-up-system-static-variables.html

namespace AssemblyCSharp
{
	static class Grid
	{
		
		
		// so, these items are available project-wide: 

		public static Map map;
		public static Controller controller;
		public static Camera camera;
		public static TurnManager turnManager;
		public static GameState gameState;
		public static PrefabLoader prefabLoader;
		
		
		
		
		// when the program launches, Grid will check that all the needed elements are in place
		// that's exactly what you do in the static constructor here:
		static Grid()
		{
			GameObject g;

			g = safeFind("Map");
			map = (Map)SafeComponent(g,"Map");

			g = safeFind("Controller");
			controller = (Controller)SafeComponent(g,"Controller");
			
			g = safeFind("Camera");
			camera = (Camera)SafeComponent(g,"Camera");

			g = safeFind("Turn Manager");
			turnManager = (TurnManager)SafeComponent(g,"TurnManager");

			g = safeFind("Game State");
			gameState = (GameState)SafeComponent(g,"GameState");

			g = safeFind("Prefab Loader");
			prefabLoader = (PrefabLoader)SafeComponent(g,"PrefabLoader");


			// PS. annoying arcane technical note - remember that really, in c# static constructors do not run
			// until the FIRST TIME YOU USE THEM.  almost certainly in any large project like this, Grid
			// would be called zillions of times by all the Awake (etc etc) code everywhere, so it is
			// a non-issue. but if you're just testing or something, it may be confusing that (for example)
			// the wake-up alert only appears just before you happen to use Grid, rather than "when you hit play"
			// you may want to call "SayHello" from the GeneralOperations.cs, just to decisively start this script.
		}
		
		
		// SayHello() has no purpose other than for developers wondering HTF you use Grid.
		// just type Grid.SayHello() anywhere in the project.
		// it is useful to add a similar routine to (example) ExpertiseSystem.cs
		// then from anywhere in the project, you can type Grid.expertiseSystem.SayHello()
		// to check everything is hooked-up properly.
		public static void SayHello()
		{
			Debug.Log("Confirming to developer that the Grid is working fine.");
		}
		
		// when Grid wakes up, it checks everything is in place - it uses these routines to do so
		private static GameObject safeFind(string s)
		{
			GameObject g = GameObject.Find(s);
			if ( g == null ) BigProblem("The " +s+ " game object is not in this scene.");
			return g;
		}
		private static Component SafeComponent(GameObject g, string s)
		{
			Component c = g.GetComponent(s);
			if ( c == null ) BigProblem("The " +s+ " component is not there.");
			return c;
		}
		private static void BigProblem(string error)
		{
			 Debug.LogError(" >>>>>>>>>>>> Cannot proceed... " +error);
			 Debug.LogError(" !!! Is it possible you just forgot to launch from scene zero, the __preEverythingScene scene.");
			Debug.Break();
		}
	}
}


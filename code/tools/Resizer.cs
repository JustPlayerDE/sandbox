﻿using System;

namespace Sandbox.Tools
{
	[Library( "tool_resizer", Title = "Resizer", Group = "construction" )]
	public partial class ResizerTool : BaseTool
	{
		public override void OnPlayerControlTick()
		{
			if ( !Host.IsServer )
				return;

			using ( Prediction.Off() )
			{
				var input = Owner.Input;
				var startPos = Owner.EyePos;
				var dir = Owner.EyeRot.Forward;

				int resizeDir;
				if ( input.Pressed( InputButton.Attack1 ) ) resizeDir = 1;
				else if ( input.Pressed( InputButton.Attack2 ) ) resizeDir = -1;
				else return;

				 var tr = Trace.Ray( startPos, startPos + dir * MaxTraceDistance )
					.Ignore( Owner )
					.Run();

				if ( !tr.Hit || !tr.Entity.IsValid() || !tr.Body.IsValid() )
					return;

				if ( tr.Entity.PhysicsGroup == null || tr.Entity.PhysicsGroup.BodyCount > 1 )
					return;

				var scale = Math.Clamp( tr.Entity.WorldScale + (0.1f * resizeDir), 0.4f, 4.0f );

				tr.Entity.WorldScale = scale;
				tr.Body.BuildMass();
				tr.Body.WakeUp();
			}	
		}
	}
}

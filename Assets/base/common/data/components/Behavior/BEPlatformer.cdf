CompDef:
{	
	NumProperties = 34;
	Name = "BEPlatformer";
	BaseClass = "Behavior";	

	Property0:
	{
		Name = "ActionButton";
		Value = "0";
		Type = "int";
	};	
	
	// Running properties
	Property1:
	{
		Name = "MaxVSpeed";
		Value = "900.0000";
		Type = "real";
	};

	Property2:
	{
		Name = "MaxHSpeed";
		Value = "900.0000";
		Type = "real";
	};

	Property3:
	{
		Name = "MovAccel";
		Value = "1.00";
		Type = "real";
	};

	Property4:
	{
		Name = "MovDecel";
		Value = "250.0000";
		Type = "real";
	};

	Property5:
	{
		Name = "Friction";
		Value = "1.0";
		Type = "real";
	};

	Property6:
	{
		Name = "RunSpeedFact";
		Value = "1.5";
		Type = "real";
	};

	Property7:
	{
		Name = "IceFrictionFact";
		Value = "1.0";
		Type = "real";
	};

	Property8:
	{
		Name = "IceMaxSpeedFact";
		Value = "250.0000";
		Type = "real";
	};
	
	Property9:
	{
		Name = "WaterFrictionFact";
		Value = "1.0";
		Type = "real";
	};

	Property10:
	{
		Name = "WaterMaxSpeedFact";
		Value = "250.0000";
		Type = "real";
	};
	
	
	// Jump properties
	Property11:
	{
		Name = "JumpSpeed";
		Value = "3000.0000";
		Type = "real";
	};

	Property12:
	{
		Name = "JumpHSpeedFact";
		Value = "1.0";
		Type = "real";
	};
	
	Property13:
	{
		Name = "NumJumps";
		Value = "1";
		Type = "int";
	};
	
	Property14:
	{
		Name = "ContinuousJump";
		Value = "false";
		Type = "bool";
	};

	Property15:
	{
		Name = "VarHeightJump";
		Value = "false";
		Type = "bool";
	};

	// Ladder properties
	Property16:
	{
		Name = "LadderSpeed";
		Value = "250.0000";
		Type = "real";
	};
	
	Property17:
	{
		Name = "AutoCenterOnLadderFact";
		Value = "10.0";
		Type = "real";
	};

	Property18:
	{
		Name = "JumpToLadders";
		Value = "false";
		Type = "bool";
	};

	Property19:
	{
		Name = "JumpFromLadders";
		Value = "false";
		Type = "bool";
	};
	
	Property20:
	{
		Name = "FallFromLadders";
		Value = "false";
		Type = "bool";
	};

	Property21:
	{
		Name = "HoldToFallFromLaddersTime";
		Value = "0.1";
		Type = "real";
	};
	
	Property22:
	{
		Name = "ShortcutLadders";
		Value = "true";
		Type = "bool";
	};
	
	// Pain properties
	Property23:
	{
		Name = "PainTime";
		Value = "3.0";
		Type = "real";
	};
	
	Property24:
	{
		Name = "PainInvTime";
		Value = "1.0";
		Type = "real";
	};

	Property25:
	{
		Name = "PainRepulsionVSpeed";
		Value = "3000.0";
		Type = "real";
	};

	Property26:
	{
		Name = "PainRepulsionHSpeed";
		Value = "250.0000";
		Type = "real";
	};
	Property27:
	{
		Name = "PainGroundFriction";
		Value = "250.0000";
		Type = "real";
	};
	
	// Crouch properties
	Property28:
	{
		Name = "CanCrouch";
		Value = "false";
		Type = "bool";
	};
	
	Property29:
	{
		Name = "CrouchSpeedFact";
		Value = "0.5";
		Type = "real";
	};
	
	Property30:
	{
		Name = "StandBVComp";
		Value = "-1";
		Type = "int";
	};
	Property31:
	{
		Name = "StandBVTopSensorComp";
		Value = "-1";
		Type = "int";
	};
	Property32:
	{
		Name = "CrouchBVComp";
		Value = "-1";
		Type = "int";
	};
	Property33:
	{
		Name = "CrouchBVTopSensorComp";
		Value = "-1";
		Type = "int";
	};
};

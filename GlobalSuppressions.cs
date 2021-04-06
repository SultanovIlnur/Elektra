// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Build", "UNT0001:The Unity message \"Update\" is empty.", Justification = "<Ожидание>", Scope = "member", Target = "~M:Cubes.Update")]
[assembly: SuppressMessage("Build", "UNT0002:Comparing tags using == is inefficient.", Justification = "<Ожидание>", Scope = "member", Target = "~M:FPSController.MakeRay")]
[assembly: SuppressMessage("Build", "UNT0001:The Unity message \"Start\" is empty.", Justification = "<Ожидание>", Scope = "member", Target = "~M:Game.Start")]
[assembly: SuppressMessage("Build", "UNT0001:The Unity message \"Update\" is empty.", Justification = "<Ожидание>", Scope = "member", Target = "~M:Game.Update")]
[assembly: SuppressMessage("Build", "UNT0014:Item is not a Unity Component", Justification = "<Ожидание>", Scope = "member", Target = "~M:Inventory.AddItem(System.String,System.Int32,System.Int32)")]
[assembly: SuppressMessage("Build", "UNT0008:Unity objects should not use null propagation.", Justification = "<Ожидание>", Scope = "member", Target = "~M:Inventory.CheckForItems")]

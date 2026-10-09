namespace FlowState.Models;

/// <summary>An inventory item that restores Energy. A non-null Risk applies the caffeine-crash debuff.</summary>
public class Consumable { public string Name = "", Desc = ""; public int Qty, Energy; public string? Risk; }

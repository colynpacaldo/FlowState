using FlowState.Models;

namespace FlowState.Services;

public record ShopItem(string Icon, string Name, string Desc, int Price);
public class Consumable { public string Name = "", Desc = ""; public int Qty, Energy; public string? Risk; }

public class GameState : IDisposable
{
    public const int MaxEnergy = 240, ExpNeeded = 500, RegenSeconds = 300;
    public event Action? Changed;
    readonly Timer _t;

    public int Energy = 185, Level = 7, Exp = 340, Gold = 1240, RegenLeft = 272;
    public DateTime? CrashUntil;
    public bool CrashActive => CrashUntil > DateTime.Now;

    public List<Mission> Missions = new()
    {
        new(){Id=1,Name="Morning Study Session",Cat=Cat.Academic,Cost=30,Claimed=true},
        new(){Id=2,Name="Creative Writing Block",Cat=Cat.Creative,Cost=20},
        new(){Id=3,Name="Morning Run — 5km",Cat=Cat.Physical,Cost=25},
        new(){Id=4,Name="Meditation & Journaling",Cat=Cat.Creative,Cost=15},
        new(){Id=5,Name="Complete Thesis Chapter III",Cat=Cat.Academic,Heavy=true,Cost=80},
        new(){Id=6,Name="Portfolio Website Redesign",Cat=Cat.Creative,Heavy=true,Cost=60},
        new(){Id=7,Name="Half Marathon Training Run",Cat=Cat.Physical,Heavy=true,Cost=70},
    };
    public List<ShopItem> Shop = new()
    {
        new("🎮","New Steam Game","Any game on your wishlist. No guilt attached.",800),
        new("🛋️","Guilt-Free Day Off","One full day of complete freedom. Zero obligations.",1200),
        new("🍽️","Restaurant Dinner","Dinner at any restaurant of your choosing.",600),
        new("📚","New Book","Any book you've been eyeing.",300),
        new("🎬","Movie Night Out","Full cinema experience. Snacks included.",200),
        new("✈️","Weekend Getaway","A short trip somewhere you've never been.",2000),
        new("👟","New Sneakers","A fresh pair for the road ahead.",1500),
        new("💆","Spa Day","A full day of rest and recovery.",1000),
    };
    public List<Consumable> Items = new()
    {
        new(){Name="Kopiko Lucky Day",Desc="A premium coffee candy that sharpens focus and restores vitality.",Qty=3,Energy=60},
        new(){Name="Cobra Energy Drink",Desc="Massive energy surge at a cost. Half regen rate for 4 hrs.",Qty=1,Energy=100,
              Risk="Cobra Energy Drink causes a 4-hour caffeine crash debuff. During this period, your Energy regen rate is halved. Use with caution."},
    };

    public GameState() => _t = new Timer(_ => Tick(), null, 1000, 1000);

    void Tick()
    {
        if (Energy < MaxEnergy && --RegenLeft <= 0) { Energy++; RegenLeft = RegenSeconds * (CrashActive ? 2 : 1); }
        Changed?.Invoke();
    }
    /// <summary>Resets progress for a brand-new account: Level 1, full energy, no gold, no missions.</summary>
    public void StartFresh()
    {
        Level = 1; Exp = 0; Energy = MaxEnergy; Gold = 0; RegenLeft = RegenSeconds;
        CrashUntil = null; Missions.Clear();
        foreach (var c in Items) c.Qty = 0;
        Changed?.Invoke();
    }
    public string RegenText => $"{RegenLeft / 60}:{RegenLeft % 60:00}";

    public bool CanClaim(Mission m) => !m.Claimed && Energy >= m.Cost;
    public void Claim(Mission m)
    {
        if (!CanClaim(m)) return;
        Energy -= m.Cost; Gold += m.Gold; Exp += m.Exp; m.Claimed = true;
        while (Exp >= ExpNeeded) { Exp -= ExpNeeded; Level++; }
        Changed?.Invoke();
    }
    public void Add(Mission m) { m.Id = Missions.Select(x => x.Id).DefaultIfEmpty(0).Max() + 1; Missions.Add(m); Changed?.Invoke(); }
    public void Remove(Mission m) { Missions.Remove(m); Changed?.Invoke(); }
    public void Redeem(ShopItem s) { if (Gold >= s.Price) { Gold -= s.Price; Changed?.Invoke(); } }
    public void Use(Consumable c)
    {
        if (c.Qty < 1) return;
        c.Qty--; Energy = Math.Min(MaxEnergy, Energy + c.Energy);
        if (c.Risk != null) CrashUntil = DateTime.Now.AddHours(4);
        Changed?.Invoke();
    }
    public void Dispose() => _t.Dispose();
}

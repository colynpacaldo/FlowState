using FlowState.Models;
using FlowState.Services;

namespace FlowState;

/// <summary>
/// Tailwind class recipes shared across pages. Every class name is written out in full
/// (never built by string interpolation) so Tailwind's scanner can see it.
/// Variants are complete strings rather than "base + override" because utility order in the
/// generated CSS, not order in the class attribute, decides which one wins.
/// </summary>
public static class Ui
{
    // ── Typography ───────────────────────────────────────────────
    public const string H1 = "font-head text-[2rem] font-bold tracking-[.08em]";
    public const string H2 = "font-head text-[1.5rem] font-bold tracking-[.06em]";
    public const string H4 = "my-2 font-head text-[1.05rem] font-bold tracking-[.06em]";
    public const string Sect = "mt-[1.6rem] mb-[.7rem] font-head text-[.8rem] font-bold tracking-[.2em] text-sect";
    public const string Lbl = "block text-[.72rem] font-medium tracking-[.14em] text-muted";
    public const string Q = "block font-head text-[1.5rem] leading-[normal] font-bold tracking-[.03em]";
    public const string St = "font-mono text-[.78rem] text-muted";
    public const string Qty = "bg-field px-2 py-[.1rem] font-mono text-[.8rem] leading-[normal] font-bold";
    public const string Bolt = "not-italic text-ember";

    // ── Buttons ──────────────────────────────────────────────────
    const string BtnCore = "inline-block cursor-pointer border-0 font-head font-bold tracking-[.06em] transition-[filter] duration-150 hover:not-disabled:brightness-110 disabled:cursor-not-allowed";
    const string Solid = "bg-gold text-[#16130c] clip-btn -skew-x-4 disabled:bg-field disabled:text-dim";
    const string Outline = "bg-transparent text-fg text-[.8rem] outline-1 outline-line -outline-offset-1";

    public const string Btn = BtnCore + " " + Solid + " px-[1.4rem] py-[.7rem] text-[1rem]";
    public const string BtnSm = BtnCore + " " + Solid + " px-[.9rem] py-[.35rem] text-[.8rem]";
    public const string BtnWide = BtnCore + " " + Solid + " mt-[1.2rem] w-full p-4 text-[1rem]";
    public const string Ghost = BtnCore + " " + Outline + " px-[1.4rem] py-[.7rem]";
    public const string GhostSm = BtnCore + " " + Outline + " px-[.9rem] py-[.35rem]";
    public const string GhostWide = BtnCore + " " + Outline + " mt-[1.2rem] w-full p-4";
    public const string GhostDanger = BtnCore + " bg-transparent text-danger text-[.8rem] outline-1 outline-danger-edge -outline-offset-1 px-[1.4rem] py-[.7rem]";

    // ── Surfaces ─────────────────────────────────────────────────
    public const string Card = "mb-[.8rem] border border-line bg-panel px-[1.2rem] py-4 clip-cut-12";
    public const string CardBal = "mb-[.8rem] border border-line bg-panel px-[1.4rem] py-[.8rem] text-right clip-cut-12";
    public const string CardItem = "border border-line bg-panel p-[1.4rem] clip-cut-12";
    public const string Quest = "border-l-[3px] bg-panel px-[1.2rem] py-4 clip-cut-14";
    const string MrowBase = "mb-[3px] flex flex-wrap items-center gap-[.9rem] border-l-[3px] bg-panel px-4 py-[.7rem]";

    public const string Risk = "my-2 border border-danger-line bg-danger/12 px-[.6rem] py-[.4rem] text-[.78rem] text-danger-soft";
    public const string TagGold = "border border-gold-dim bg-gold/12 px-[.6rem] py-[.1rem] font-bold text-gold";
    public const string TagRisk = "border border-danger-line bg-danger/12 px-[.6rem] py-[.15rem] text-[.78rem] text-danger-soft";
    public const string RiskBox = "mt-[.7rem] border-l-2 border-danger pl-[.7rem] text-[.78rem] text-danger-soft";

    // ── Forms ────────────────────────────────────────────────────
    public const string Label = "mt-[1.1rem] mb-[.4rem] block text-[.75rem] tracking-[.12em] text-muted";
    public const string Input = "mt-2 w-full border border-line bg-field p-4 font-body text-[1.05rem] leading-[normal] text-fg placeholder:text-[#6d7480] aria-invalid:border-danger";
    public const string Err = "mt-[.4rem] block text-[.78rem] tracking-[0] text-danger-soft normal-case";
    public const string Hint = "mt-[.4rem] block text-[.78rem] tracking-[0] text-dim normal-case";
    public const string Eye = "absolute top-1/2 right-[.8rem] grid -translate-y-1/2 cursor-pointer place-items-center border-0 bg-transparent text-[1.1rem] text-muted hover:text-fg";
    public const string LinkBtn = "cursor-pointer text-muted underline hover:text-fg";

    // ── Navigation (NavLink appends the "active" class) ──────────
    public const string NavSide = "flex gap-[.8rem] border-l-[3px] border-transparent px-[1.2rem] py-[.8rem] text-muted [&.active]:border-l-gold [&.active]:bg-gold/6 [&.active]:font-semibold [&.active]:text-gold [&:not(.active):hover]:text-fg";
    public const string NavBottom = "flex-1 py-2 text-center text-[.68rem] text-muted [&.active]:text-gold";
    public const string NavIconSide = "w-[1.2rem] text-center not-italic";
    public const string NavIconBottom = "mx-auto block w-[1.2rem] text-center text-[1.1rem] not-italic";

    // ── Category-aware recipes ───────────────────────────────────
    public static string Mrow(Cat c, bool done = false) =>
        MrowBase + " " + Edge(c) + (done ? " opacity-50" : "");

    public static string QuestCard(Cat c) => Quest + " " + Edge(c);

    static string Edge(Cat c) => c switch
    {
        Cat.Academic => "border-academic",
        Cat.Creative => "border-creative",
        _ => "border-physical",
    };

    const string ChipCore = "border border-line px-2 py-[.1rem] font-head text-[.72rem] font-semibold tracking-[.06em]";
    public const string ChipPlain = ChipCore + " bg-field text-muted";
    public static string Chip(Cat c) => ChipCore + " " + c switch
    {
        Cat.Academic => "bg-academic/15 text-academic",
        Cat.Creative => "bg-creative/15 text-creative",
        _ => "bg-physical/15 text-physical",
    };

    const string CatCore = "font-head text-[.72rem] leading-[normal] font-semibold tracking-[.1em]";
    public static string CatText(Cat c) => CatCore + " " + c switch
    {
        Cat.Academic => "text-academic",
        Cat.Creative => "text-creative",
        _ => "text-physical",
    };

    // ── Segmented selectors (New Mission) ────────────────────────
    const string OptCore = "cursor-pointer border p-[.9rem] font-head text-[.95rem] font-bold tracking-[.06em]";
    const string OptOff = OptCore + " border-line bg-canvas text-muted";
    public static string OptCat(Cat c, bool on) => !on ? OptOff : OptCore + " " + c switch
    {
        Cat.Academic => "border-academic bg-academic/12 text-academic",
        Cat.Creative => "border-creative bg-creative/12 text-creative",
        _ => "border-physical bg-physical/12 text-physical",
    };
    public static string OptGold(bool on) => on ? OptCore + " border-gold-dim bg-panel text-gold" : OptOff;

    const string CostCore = "cursor-pointer border-0 py-[.7rem] font-mono text-[.85rem] leading-[normal]";
    public static string Cost(bool on) => CostCore + (on ? " bg-gold font-bold text-[#16130c]" : " bg-field text-muted");
}

# FlowState

## Core Mechanics & Game Loop
The application functions as a resource management game. Every real-world task requires a specific amount of "Energy" (your mental/physical bandwidth), which is capped at a maximum value (e.g., 240/240).

- Dailies (Fixed Tasks): Low-cost, repeatable tasks (e.g., "Review CS notes," "Rollerblade for 30 mins") that yield account EXP and a premium currency.

- Heavy Quests (One-off Tasks): High-cost tasks (e.g., "Draft museum pixel art background," "Debug JavaFX layout") that drain significant Energy but offer massive EXP payouts.

- The Reward System: The premium currency earned from Dailies can be spent in a "Shop" component to redeem real-life rewards you set for yourself, such as buying a new steam game or taking a guilt-free day off.

## Blazor Component & CRUD Architecture
You can structure the Blazor web assembly project using distinct components for each CRUD operation to manage state efficiently.

- Create (NewMission.razor): A form to generate a new task. You input the TaskName, select a Category (Academic, Creative, Physical), and assign an EnergyCost (10 to 80 points based on difficulty).

- Read (Dashboard.razor): The main interface displaying your current Energy bar, your level, and your active mission list. You can use a C# Timer running in the background of your Blazor layout to slowly regenerate 1 Energy point every 6 minutes, mimicking actual gacha mechanics.

- Update (MissionItem.razor): An interactive card for each task. Clicking a "Claim" button triggers an EventCallback to the parent component, deducting the specific EnergyCost from your current pool and adding EXP to your progress bar.

- Delete (MissionManager.razor): A settings panel to permanently delete or archive daily routines that are no longer relevant to your current semester schedule.

## The "Consumables" Inventory System
Sometimes you need to complete a 60-Energy task, but your bar is sitting at 20. This is where your real-world caffeine intake translates into in-game items. You can build an inventory database where logging a drink immediately alters your C# state.

- Kopiko Lucky Day: A standard consumable. Clicking "Use" in the Blazor inventory adds an instant +60 Energy to your current pool, allowing you to bypass the regeneration timer and tackle another task.

- Cobra Energy (Variant): A high-risk, high-reward consumable. Clicking this restores +100 Energy instantly for a massive study session, but triggers a C# boolean that cuts your natural Energy regeneration rate in half for the next four hours to simulate the caffeine crash.

## Styling (Tailwind CSS v4)
Styles are utility classes in the `.razor` files. Design tokens (colors, fonts, breakpoints) live in `Styles/tailwind.css`; repeated class recipes (buttons, cards, chips, category colors) live in `Ui.cs`.

- Requires Node.js 18+. First time: `npm install && npm run css:build`
- While developing, run `npm run css:watch` next to `dotnet watch`
- `dotnet build` regenerates `wwwroot/css/app.css` automatically (skip with `-p:SkipTailwind=true`)
- Write class names out in full (`text-academic`, never `text-@cat`) so Tailwind can detect them

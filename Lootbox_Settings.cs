// Decompiled with JetBrains decompiler
// Type: Toolkit___DailyLootboxes.Lootbox_Settings
// Assembly: Toolkit - DailyLootboxes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF458F6E-F5B0-4125-B6F8-E9058A536CAA
// Assembly location: C:\Program Files (x86)\Steam\steamapps\workshop\content\294100\1720021578\Assemblies\Toolkit - DailyLootboxes.dll

using UnityEngine;
using Verse;

namespace Toolkit___DailyLootboxes
{
  public class Lootbox_Settings : ModSettings
  {
    public static IntRange RandomCoinRange = new IntRange(250, 750);
    public static int LootboxesPerDay = 1;
    public static bool ShowWelcomeMessage = true;
    public static bool ForceOpenAllLootboxesAtOnce = false;

    public void DoWindowContents(Rect inRect)
    {
      Listing_Standard listingStandard = new Listing_Standard();
      listingStandard.Begin(inRect);
      listingStandard.ColumnWidth = inRect.width / 2f;
            listingStandard.CheckboxLabeled("Show welcome message?", ref Lootbox_Settings.ShowWelcomeMessage);
      listingStandard.Gap();
      listingStandard.Label("Amount of coins given per lootbox range");
      listingStandard.IntRange(ref Lootbox_Settings.RandomCoinRange, 1, 10000);
      listingStandard.Gap();
      string buffer = Lootbox_Settings.LootboxesPerDay.ToString();
      listingStandard.TextFieldNumericLabeled<int>("Lootboxes per day:", ref Lootbox_Settings.LootboxesPerDay, ref buffer, max: 20f);
      listingStandard.Gap();
            listingStandard.CheckboxLabeled("Force Viewers to Open all Lootboxes at Once", ref Lootbox_Settings.ForceOpenAllLootboxesAtOnce);
      listingStandard.End();
    }

    public override void ExposeData()
    {
      Scribe_Values.Look<IntRange>(ref Lootbox_Settings.RandomCoinRange, "RandomCoinRange", new IntRange(250, 750));
      Scribe_Values.Look<int>(ref Lootbox_Settings.LootboxesPerDay, "LootboxesPerDay", 1);
      Scribe_Values.Look<bool>(ref Lootbox_Settings.ShowWelcomeMessage, "ShowWelcomeMessage", true);
      Scribe_Values.Look<bool>(ref Lootbox_Settings.ForceOpenAllLootboxesAtOnce, "ForceOpenAllLootboxesAtOnce");
    }
  }
}

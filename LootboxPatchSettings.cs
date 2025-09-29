// Decompiled with JetBrains decompiler
// Type: Toolkit___DailyLootboxes.LootboxPatchSettings
// Assembly: Toolkit - DailyLootboxes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF458F6E-F5B0-4125-B6F8-E9058A536CAA
// Assembly location: C:\Program Files (x86)\Steam\steamapps\workshop\content\294100\1720021578\Assemblies\Toolkit - DailyLootboxes.dll

using TwitchToolkit.Settings;
using UnityEngine;
using Verse;

namespace Toolkit___DailyLootboxes
{
  public class LootboxPatchSettings : ToolkitWindow
  {
    public LootboxPatchSettings(Mod mod)
      : base(mod)
    {
      this.Mod = mod;
    }

    public override void DoWindowContents(Rect inRect) => this.Mod.GetSettings<Lootbox_Settings>().DoWindowContents(inRect);
  }
}

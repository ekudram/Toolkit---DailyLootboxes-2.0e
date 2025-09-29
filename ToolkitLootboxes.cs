// Decompiled with JetBrains decompiler
// Type: Toolkit___DailyLootboxes.ToolkitLootboxes
// Assembly: Toolkit - DailyLootboxes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF458F6E-F5B0-4125-B6F8-E9058A536CAA
// Assembly location: C:\Program Files (x86)\Steam\steamapps\workshop\content\294100\1720021578\Assemblies\Toolkit - DailyLootboxes.dll

using TwitchToolkit.Settings;
using UnityEngine;
using Verse;

namespace Toolkit___DailyLootboxes
{
  public class ToolkitLootboxes : Mod
  {
    public ToolkitLootboxes(ModContentPack content)
      : base(content)
    {
      this.GetSettings<Lootbox_Settings>();
      Settings_ToolkitExtensions.RegisterExtension(new ToolkitExtension((Mod) this, typeof (LootboxPatchSettings)));
    }

    public override string SettingsCategory() => "Toolkit - Lootboxes";

    public override void DoSettingsWindowContents(Rect inRect) => this.GetSettings<Lootbox_Settings>().DoWindowContents(inRect);
  }
}

// Decompiled with JetBrains decompiler
// Type: Toolkit___DailyLootboxes.Lootbox
// Assembly: Toolkit - DailyLootboxes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF458F6E-F5B0-4125-B6F8-E9058A536CAA
// Assembly location: C:\Program Files (x86)\Steam\steamapps\workshop\content\294100\1720021578\Assemblies\Toolkit - DailyLootboxes.dll

using ToolkitCore;
using TwitchToolkit;
using Verse;

namespace Toolkit___DailyLootboxes
{
  public static class Lootbox
  {
    public static void OpenLootbox(TwitchMessageWrapper twitchMessage, string username)
    {
      if (Lootbox_Settings.ForceOpenAllLootboxesAtOnce)
      {
        Lootbox.OpenAllLootboxes(twitchMessage, username);
      }
      else
      {
        LootboxComponent component = Current.Game.GetComponent<LootboxComponent>();
        string str1 = "@" + username + " you open a lootbox and discover: ";
        int coins = Rand.Range(Lootbox_Settings.RandomCoinRange.min, Lootbox_Settings.RandomCoinRange.max);
        string str2 = coins.ToString();
        string message = str1 + str2 + " coins";
        Viewer viewer = Viewers.GetViewer(username);
        viewer.GiveViewerCoins(coins);
        component.ViewersLootboxes[viewer.username]--;
        TwitchWrapper.SendChatMessage(message);
      }
    }

    public static void OpenAllLootboxes(TwitchMessageWrapper message, string username)
    {
      string str = "@" + username + " you open all your lootboxes and discover: ";
      LootboxComponent component = Current.Game.GetComponent<LootboxComponent>();
      int num = component.HowManyLootboxesDoesViewerHave(username);
      int coins = 0;
      for (int index = 0; index < num; ++index)
        coins += Rand.Range(Lootbox_Settings.RandomCoinRange.min, Lootbox_Settings.RandomCoinRange.max);
      string message1 = str + coins.ToString() + " coins";
      Viewer viewer = Viewers.GetViewer(username);
      viewer.GiveViewerCoins(coins);
      component.ViewersLootboxes[viewer.username] = 0;
      TwitchWrapper.SendChatMessage(message1);
    }
  }
}

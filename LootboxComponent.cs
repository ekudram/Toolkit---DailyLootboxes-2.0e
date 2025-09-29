/*
 * Project: DailyLootBoxes 2.0e
 * File: LootboxComponent.cs
 * 
 */
using System;
using System.Collections.Generic;
using ToolkitCore;
using ToolkitCore.Controllers;
using TwitchToolkit;
using Verse;

namespace Toolkit___DailyLootboxes
{
  public class LootboxComponent : TwitchToolkit.TwitchInterfaceBase
  {
    public DateTime today = DateTime.Now;
    public long todayFileTime;
    public List<string> ViewersWhoHaveRecievedLootboxesToday = new List<string>();
    public Dictionary<string, long> ViewersLastSeenDate = new Dictionary<string, long>();
    public Dictionary<string, int> ViewersLootboxes = new Dictionary<string, int>();

    public LootboxComponent(Game game)
    {
      if (this.ViewersWhoHaveRecievedLootboxesToday != null)
        return;
      this.ViewersWhoHaveRecievedLootboxesToday = new List<string>();
    }

    public override void GameComponentTick()
    {
      if (Find.TickManager.TicksGame % 20000 != 0)
        return;
      DateTime dateTime = DateTime.FromFileTime(this.todayFileTime);
      int dayOfYear1 = dateTime.DayOfYear;
      dateTime = DateTime.Now;
      int dayOfYear2 = dateTime.DayOfYear;
      if (dayOfYear1 == dayOfYear2)
        return;
      this.ViewersWhoHaveRecievedLootboxesToday = new List<string>();
      this.today = DateTime.Now;
      this.todayFileTime = this.today.ToFileTime();
    }

    public override void ParseMessage(TwitchMessageWrapper twitchMessage)
    {
      Viewer viewer1 = Viewers.GetViewer(twitchMessage.Username);
      if (this.IsViewerOwedLootboxesToday(viewer1.username.ToLower()))
        this.AwardViewerDailyLootboxes(viewer1.username.ToLower());
      if (twitchMessage.Message.StartsWith("!openlootbox"))
      {
        if (this.HowManyLootboxesDoesViewerHave(viewer1.username) > 0)
          Lootbox.OpenLootbox(twitchMessage, viewer1.username.ToLower());
        else
          TwitchWrapper.SendChatMessage("@" + viewer1.username + " you do not have any lootboxes.");
      }
      if (twitchMessage.Message.StartsWith("!lootboxes"))
      {
        int num = this.HowManyLootboxesDoesViewerHave(viewer1.username);
        string str1 = num > 1 || num == 0 ? "es" : "";
        string str2 = num > 1 ? " Use !openlootbox" + (!ToolkitSettings.UseSeparateChatRoom || ToolkitSettings.AllowBothChatRooms ? "" : " in the separate chat room.") : "";
        TwitchWrapper.SendChatMessage(string.Format("@{0} you currently have {1} lootbox{2}.{3}", (object) viewer1.username, (object) num, (object) str1, (object) str2));
      }
      if (!twitchMessage.Message.StartsWith("!givelootbox") || !Viewer.IsModerator(viewer1.username) && !ViewerController.GetViewer(viewer1.username).IsBroadcaster)
        return;
      string[] strArray = twitchMessage.Message.Split(' ');
      if (strArray.Length < 2)
        return;
      int result = 1;
      if (strArray.Length > 2)
        int.TryParse(strArray[2], out result);
      string str = result > 1 ? "es" : "";
      strArray[1] = strArray[1].Replace("@", "");
      Viewer viewer2 = Viewers.GetViewer(strArray[1]);
      this.GiveViewerLootbox(viewer2.username, result);
      TwitchWrapper.SendChatMessage(string.Format("@{0} you have received {1} lootbox{2} from {3}.", (object) viewer2.username, (object) result, (object) str, (object) viewer1.username));
    }

    public void WelcomeMessage(string username) => TwitchWrapper.SendChatMessage(string.Format("@{0} Welcome to {1}'s Stream, You currently have {2} Lootbox(es) to open. Use !openlootbox", (object) username, (object) ToolkitSettings.Channel, (object) this.HowManyLootboxesDoesViewerHave(username)));

    public void AwardViewerDailyLootboxes(string username)
    {
      this.ViewersWhoHaveRecievedLootboxesToday.Add(username);
      this.LogViewerLastSeen(username);
      this.GiveViewerLootbox(username, Lootbox_Settings.LootboxesPerDay);
      if (!Lootbox_Settings.ShowWelcomeMessage)
        return;
      this.WelcomeMessage(username);
    }

    public void GiveViewerLootbox(string username, int amount = 1)
    {
      if (this.ViewersLootboxes.ContainsKey(username))
        this.ViewersLootboxes[username] += amount;
      else
        this.ViewersLootboxes.Add(username, amount);
    }

    private bool IsViewerOwedLootboxesToday(string username)
    {
      if (this.ViewersWhoHaveRecievedLootboxesToday == null)
        this.ViewersWhoHaveRecievedLootboxesToday = new List<string>();
      return !this.ViewersWhoHaveRecievedLootboxesToday.Contains(username) && this.IsViewerOwedLootboxesLookup(username);
    }

    private bool IsViewerOwedLootboxesLookup(string username)
    {
      if (this.ViewersLastSeenDate == null)
        this.ViewersLastSeenDate = new Dictionary<string, long>();
      if (this.ViewersLootboxes == null)
        this.ViewersLootboxes = new Dictionary<string, int>();
      return !this.IsViewerInLastSeenList(username) || this.ViewerLastSeenAt(username).DayOfYear != DateTime.Now.DayOfYear;
    }

    public void LogViewerLastSeen(string username)
    {
      if (this.ViewersLastSeenDate.ContainsKey(username))
        this.ViewersLastSeenDate[username] = DateTime.Now.ToFileTime();
      else
        this.ViewersLastSeenDate.Add(username, DateTime.Now.ToFileTime());
    }

    public bool IsViewerInLastSeenList(string username) => this.ViewersLastSeenDate.ContainsKey(username);

    private DateTime ViewerLastSeenAt(string username) => DateTime.FromFileTime(this.ViewersLastSeenDate[username]);

    public bool DoesViewerHaveLootboxes(string username) => this.ViewersLootboxes.ContainsKey(username) && this.ViewersLootboxes[username] > 0;

    public int HowManyLootboxesDoesViewerHave(string username) => this.ViewersLootboxes.ContainsKey(username) ? this.ViewersLootboxes[username] : 0;

    public override void ExposeData()
    {
            Scribe_Collections.Look<string>(ref this.ViewersWhoHaveRecievedLootboxesToday, "ViewersWhoHaveRecievedLootboxesToday", LookMode.Value);
      Scribe_Collections.Look<string, long>(ref this.ViewersLastSeenDate, "ViewersLastSeenDate", LookMode.Value, LookMode.Value);
      Scribe_Collections.Look<string, int>(ref this.ViewersLootboxes, "ViewersLootboxes", LookMode.Value, LookMode.Value);
      Scribe_Values.Look<long>(ref this.todayFileTime, "todayFileTime");
    }
  }
}

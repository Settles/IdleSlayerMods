using System.Collections.Generic;
using Il2Cpp;

namespace AutoAdventurer.Quests;

/// <summary>
/// Rebuilds the quest-panel snapshot from the per-category lists that
/// QuestsList still exposes. The game removed QuestsList.lastScrollListData,
/// so calling it throws MissingMethodException at runtime.
/// </summary>
internal static class QuestListSnapshot
{
    /// <summary>
    /// Returns the visible quest list in panel order, or null when the panel
    /// has not built its cache yet.
    /// </summary>
    internal static List<Quest> Read(QuestsList questsList)
    {
        if (questsList == null || questsList.mainQuests == null) return null;

        var result = new List<Quest>();
        AddAll(result, questsList.mainQuests);
        AddAll(result, questsList.eventQuests);
        AddAll(result, questsList.weeklyQuests);
        AddAll(result, questsList.dailyQuests);

        var collabOrder = questsList.collabOrder;
        var collabQuests = questsList.collabQuests;
        if (collabOrder != null && collabQuests != null)
        {
            for (int index = 0; index < collabOrder.Count; index++)
            {
                if (collabQuests.TryGetValue(collabOrder[index], out var quests))
                    AddAll(result, quests);
            }
        }

        return result;
    }

    private static void AddAll(
        List<Quest> result, Il2CppSystem.Collections.Generic.List<Quest> source)
    {
        if (source == null) return;
        for (int index = 0; index < source.Count; index++)
        {
            Quest quest = source[index];
            if (quest != null) result.Add(quest);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordBookCardInitializer : CardBase
{
    public override void Initialize(GameName _game, string _categoryKey, string cardKey, CardData data)
    {
        base.Initialize(_game, _categoryKey, cardKey, data);

        selectBtn.onClick.AddListener(Active);
    }

    public void Active()
    {
        Signals.WordBookCardSelect.Invoke(Key);
    }
}

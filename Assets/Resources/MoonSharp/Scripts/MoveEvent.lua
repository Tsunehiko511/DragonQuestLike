﻿  function main()
    player.SetCanInputFlag(false)
    player.MoveTo("right", 3)
    coroutine.yield()
    player.MoveTo("down", 3)
    npc.MoveTo("up", 3)
    coroutine.yield()
    player.MoveTo("up", 3)
    npc.MoveTo("down", 3)
    coroutine.yield()
    player.MoveTo("down", 3)
    npc.MoveTo("up", 3)
    coroutine.yield()
    player.MoveTo("up", 2)
    npc.MoveTo("down", 2)
    coroutine.yield()
    message.ShowMessage("なにをしておる？？")
    coroutine.yield()
    player.MoveTo("right", 7)
    npc.MoveTo("right", 6)
    coroutine.yield()
    message.ShowMessage("王「今回来てもらったのは他でもない",true)
    coroutine.yield()
    message.ShowMessage("王「スタジオしまづ討伐についてじゃ", true)
    coroutine.yield()
    message.ShowMessage("王「最近, Unityでゲームを作る方法を\n教えてるらしいのだが...", true)
    coroutine.yield()
    message.ShowMessage("王「全く何者なのかよくわからん.", true)
    coroutine.yield()
    message.ShowMessage("王「このままでは国の治安が悪くなるので,消してまいれ！", true)
    coroutine.yield()
    message.ShowMessage("*「はは\n*「はは(まじかよ...オレだよ)")
    coroutine.yield()
    player.Wait(0.5);
    coroutine.yield()
    player.MoveTo("left", 6)
    npc.MoveTo("left", 6)
    coroutine.yield()
    player.MoveTo("down", 1)
    npc.MoveTo("down", 1)
    coroutine.yield()
    door.Open()
    coroutine.yield()
    player.Wait(0.5);
    coroutine.yield()
    player.MoveTo("down", 5)
    npc.MoveTo("down", 5)
    coroutine.yield()
    player.SetCanInputFlag(true)
    coroutine.yield()
  end
function OnKeyStay(keycode)
    if keycode == 'LeftArrow' then
        player['trans'].move(-1, 0)
    end
    if keycode == 'RightArrow' then
        player['trans'].move(1, 0)
    end
    if keycode == 'UpArrow' then
        player['trans'].move(0, 2)
    end
    if keycode == 'DownArrow' then
        player['trans'].move(0, -1)
    end
end

function OnCollisionEnter(name)
    print(name)
end
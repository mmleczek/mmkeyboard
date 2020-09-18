# Examples
#### Lua
```
@allow_to_walk - defines if player can walk while isn't typing any text
@maxlen - defines max possible lenght of text in textarea
```
```lua
exports.mmkeyboard:Show(@allow_to_walk, @maxlen, function(text)
    print(text)
end)

exports.mmkeyboard:Show(false, 255, function(text)
    print(text)
end)
```
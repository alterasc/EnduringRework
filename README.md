# Enduring rework - mod for Pathfinder: Wrath of the Righteous

Changes Enduring spells and Greater Enduring Spells mythic abilities to be the following

Enduring Spells
```
Prerequisite: Metamagic (Extend Spell)

You've learned a way to prolong the effects of your beneficial extended spells.
Benefit: Effects of your spells on your allies cast with extend metamagic applied that should last longer than an hour but shorter than 24 hours now last 24 hours. Effects that should last longer than 10 minutes (but no longer than 1 hour) last 1 hour. Effects that should last longer than 1 minute  (but no longer than 10 minutes) last 10 minutes.
```

Greater Enduring Spells
```
You've mastered a way to prolong your beneficial spells.
Benefit: In addition to existing benefits now effects of your spells on your allies cast with extend metamagic that should last longer than an hour are permanent.
```

### Note:
**Unlike base game where "longer than X" is "longer or equal" here it's actually longer as written.**

So 1 minute extended spell will stay 1 minute. 1 minute 12 seconds will become 10 minutes.

Cave Fangs, Call Lightning, Call Lightning Storm won't become permanent, at max 1 day. Sorry if you already theorized this.

Does not introduce save dependency, so you can remove it any time.

If used together with Tabletop-Reworks, disable TT-Reworks changes to Enduring Spells. 
I've set up my mod to load afterwards and unconditionally overwrite TTT changes, but it's better to be safe.

### My subjective reasoning behind this change:
ES without GES is worthless. No one needs to make 1hour+ long spells last longer.   
While it's possible to throw Mage Armor and Freedom Of Movement on everyone and then rest to recover spell slots but number of such spells is relatively low and it's anything but interesting gameplay (especially if done on prepared caster)

So ES needed something to make it stand by itself.

GES meanwhile is where most of the benefit is. It immediately allows min/level spell to last a day and at CL25 it includes round/level spells.   
My personal problem with that however is that 2 Mythic Abilities is too high of a price to make minute/level spells last longer and 25 CL is too high of a price to extend round/level price. If you're not playing merged spellbook character (I never do) than 25 CL is a very late game thing, unless you do some minmaxing to push your CL.

So I pushed most of the duration extending benefit to ES and to counter balance it a bit, only spells with Extend are affected. 
Now there's tangible benefits in extending spells that have low duration, but they won't be there forever with you.

GES pushes some long duration buffs into permanent buffs. We both know your Barkskin will not suddenly run out ever. So let it always be there and not waste any more time on that.

Potentially with my changes you (player) can have a character that exists to just throw these buffs permanently and then never ever be present in a party, but I'm not going to try to introduce logic to remove buffs if original caster is not present. As far I'm concerned this is a cheese that's too hard to fix, so it stays.

Is my overhaul of ES/GES makes it stronger? I think so. But as I never play with them, because I think they are weak, I think buff is what they need. Does it go into overpowered territory? I don't think so, our opinions can differ.
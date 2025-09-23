// RoundEndGravityPlugin.cs
using System;
using System.Collections.Generic;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;

namespace RoundEndGravity;

[MinimumApiVersion(80)]
public class RoundEndGravityPlugin : BasePlugin, IPluginConfig<RoundEndGravityConfig>
{
    public override string ModuleName => "Round End Gravity";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "levice1";
    public override string ModuleDescription => "Lowers gravity at round end, restores at next round start; disabled during warmup.";

    public required RoundEndGravityConfig Config { get; set; }
    private bool _isWarmup;

    public override void Load(bool hotReload)
    {
        // Warmup
        RegisterEventHandler<EventRoundAnnounceWarmup>(OnWarmupAnnounce, HookMode.Post);
        RegisterEventHandler<EventWarmupEnd>(OnWarmupEnd, HookMode.Post);

        // Rounds
        RegisterEventHandler<EventRoundEnd>(OnRoundEnd, HookMode.Post);
        RegisterEventHandler<EventRoundStart>(OnRoundStart, HookMode.Post);
        Server.PrintToConsole($"[{ModuleName}] Loaded v{ModuleVersion}");
    }

    public void OnConfigParsed(RoundEndGravityConfig config)
    {
        Config = config;
        Server.PrintToConsole($"[{ModuleName}] Config: Normal={Config.NormalGravity}, End={Config.RoundEndGravity}");
    }

    public override void Unload(bool hotReload)
    {
        DeregisterEventHandler<EventRoundAnnounceWarmup>(OnWarmupAnnounce, HookMode.Post);
        DeregisterEventHandler<EventWarmupEnd>(OnWarmupEnd, HookMode.Post);
        DeregisterEventHandler<EventRoundEnd>(OnRoundEnd, HookMode.Post);
        DeregisterEventHandler<EventRoundStart>(OnRoundStart, HookMode.Post);
        Server.PrintToConsole($"[{ModuleName}] Unloaded");
    }

    // --- Warmup ---
    private HookResult OnWarmupAnnounce(EventRoundAnnounceWarmup @event, GameEventInfo info)
    {
        _isWarmup = true;
        Server.PrintToConsole($"[{ModuleName}] Warmup start -> gravity {Config.NormalGravity}");
        return HookResult.Continue;
    }

    private HookResult OnWarmupEnd(EventWarmupEnd @event, GameEventInfo info)
    {
        _isWarmup = false;
        Server.PrintToConsole($"[{ModuleName}] Warmup end -> gravity {Config.NormalGravity}");
        return HookResult.Continue;
    }


    private HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        if (_isWarmup) return HookResult.Continue;
        SetGravity(Config.RoundEndGravity);
        Server.PrintToConsole($"[{ModuleName}] Round end -> gravity {Config.RoundEndGravity}");
        return HookResult.Continue;
    }

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        SetGravity(Config.NormalGravity);
        Server.PrintToConsole($"[{ModuleName}] Round start -> gravity {Config.NormalGravity}");
        return HookResult.Continue;
    }

    private void SetGravity(int value)
    {
        Server.ExecuteCommand($"sv_gravity {value}");
    }
}

public class RoundEndGravityConfig : BasePluginConfig
{
    public int RoundEndGravity { get; set; } = 200;
    public int NormalGravity   { get; set; } = 800;
}

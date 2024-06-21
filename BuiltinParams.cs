using System.Collections.Generic;

namespace VRCDebug
{
    public class BuiltinParams
    {
        // VRC's built-in parameters
        public static readonly Dictionary<string,string> List = new Dictionary<string,string>
        {
            { "IsLocal", "True if the avatar is being worn locally, false otherwise | Sync: None" },
            { "Viseme", "Oculus viseme index (0-14). When using Jawbone/Jawflap, range is 0-100 indicating volume | Sync: Speech" },
            { "Voice", "Microphone volume (0.0-1.0) | Sync: Speech" },
            { "GestureLeft", "Gesture from L hand control (0-7) | Sync: IK" },
            { "GestureRight", "Gesture from R hand control (0-7) | Sync: IK" },
            { "GestureLeftWeight", "Analog trigger L (0.0-1.0) | Sync: Playable" },
            { "GestureRightWeight", "Analog trigger R (0.0-1.0) | Sync: Playable" },
            { "AngularY", "Angular velocity on the Y axis | Sync: IK" },
            { "VelocityX", "Lateral move speed in m/s | Sync: IK" },
            { "VelocityY", "Vertical move speed in m/s | Sync: IK" },
            { "VelocityZ", "Forward move speed in m/s | Sync: IK" },
            { "VelocityMagnitude", "Total magnitude of velocity | Sync: IK" },
            { "Upright", "How \"upright\" you are. 0 is prone, 1 is standing straight up | Sync: IK" },
            { "Grounded", "True if player touching ground | Sync: IK" },
            { "Seated", "True if player in station | Sync: IK" },
            { "AFK", "Is player unavailable (HMD proximity sensor / End key) | Sync: IK" },
            { "TrackingType", "If the value is 3, 4, or 6 while VRMode is 1, the value is indicating how many tracked points the wearer of the avatar has enabled and currently tracked. If the value is 0, 1, or 2 while VRMode is 1, the value indicates that the avatar is still initializing. | Sync: Playable" },
            { "VRMode", "Returns 1 if the user is in VR, 0 if they are not | Sync: IK" },
            { "MuteSelf", "Returns true if the user has muted themselves, false if unmuted | Sync: Playable" },
            { "InStation", "Returns true if the user is in a station, false if not | Sync: IK" },
            { "Earmuffs", "Returns true if the user's Earmuff feature is on, false if not | Sync: Playable" },
            { "IsOnFriendsList", "Returns true if the user viewing the avatar is friends with the user wearing it. false locally. | Sync: Other" },
            { "AvatarVersion", "Returns 3 if the avatar was built using VRChat's SDK3 (2020.3.2) or later, 0 if not. | Sync: IK" },
            { "Supine", "Not implemented yet. currently does nothing and never changes values." },
            { "GroundProximity", "Not implemented yet. currently does nothing and never changes values." },
            { "ScaleModified", "Returns true if the user is scaled using avatar scaling, false if the avatar is at its default size. | Sync: Playable" },
            { "ScaleFactor", "Relation between the avatar's default height and the current height. An avatar with a default eye-height of 1m scaled to 2m will report 2. | Sync: Playable" },
            { "ScaleFactorInverse", "Inverse relation (1/x) between the avatar's default height and the current height. An avatar with a default eye-height of 1m scaled to 2m will report 0.5. Might be inaccurate at extremes. | Sync: Playable" },
            { "EyeHeightAsMeters", "The avatar's eye height in meters. | Sync: Playable" },
            { "EyeHeightAsPercent", "Relation of the avatar's eye height in meters relative to the default scaling limits (0.2-5.0). An avatar scaled to 2m will report (2.0 - 0.2) / (5.0 - 0.2) = 0.375. | Sync: Playable" }
        };
    }
}

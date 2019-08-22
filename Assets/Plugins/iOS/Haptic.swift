import AudioToolbox

class Haptic: NSObject {
    @objc static func light() {
        AudioServicesPlaySystemSound(1003);
        AudioServicesDisposeSystemSoundID(1003);
    }
}
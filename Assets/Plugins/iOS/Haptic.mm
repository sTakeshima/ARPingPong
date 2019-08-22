#import "unityswift-Swift.h"
 
extern "C" {
    void light() {
        // Call swift code
        [Haptic light];
    }
}
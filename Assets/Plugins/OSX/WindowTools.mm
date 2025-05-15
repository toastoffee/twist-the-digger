// Mac code (in Objective-C++) for WindowTools plugin.

#include <CoreFoundation/CoreFoundation.h>

extern "C" {

    void NSLog(CFStringRef format, ...);

    void TestEntryPoint() {
        NSLog(CFSTR("Hello world!  This is a test."));
    }

    int TestEntryPoint2() {
        return 42;
    }
}
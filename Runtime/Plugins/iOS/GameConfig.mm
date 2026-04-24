#import <Foundation/Foundation.h>

extern "C"
{
    const char* GetInfoPlistConfigValue(const char* key)
    {
        @autoreleasepool {
            NSString* nsKey = [NSString stringWithUTF8String:key];
            id value = [[[NSBundle mainBundle] infoDictionary] objectForKey:nsKey];
            if (value == nil) {
                return NULL;
            }
            
            if ([value isKindOfClass:[NSString class]]) {
                return strdup([value UTF8String]);
            }
            else if ([value isKindOfClass:[NSNumber class]]) {
                NSString *numStr = [((NSNumber*)value) stringValue];
                return strdup([numStr UTF8String]);
            }
            else {
                return NULL;
            }
        }
    }
}

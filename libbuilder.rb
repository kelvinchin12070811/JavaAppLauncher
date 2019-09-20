#!/usr/env/bin ruby

config = 'Debug'
platform = 'Win32'

config = ARGV[0] if ARGV.length >= 1
platform = ARGV[1] if ARGV.length >= 2

system "devenv /Build \"#{ config }|#{ platform }\" ziplib/Source/ZipLib/ZipLib.vcxproj"
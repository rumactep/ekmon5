import struct

def ushort2float(ff) :
    # ff = (sh1, sh2)
    ps = struct.pack("HH", *ff) # "HH" for ushort ushort, "hh" for short short
    uf = struct.unpack("f", ps)[0]
    return uf

def float2ushort(ff):
    ps = struct.pack("f", ff)
    uss = struct.unpack("HH", ps)
    (sh1, sh2) = (uss[0], uss[1])
    return (sh1, sh2)

def printpair(sh1, sh2, ff):
    print(f"{sh1}, {sh2} <-> {ff}")


sh1 = 39322 
sh2 = 16781

ff = ushort2float((sh1, sh2))
printpair(sh1, sh2, ff)
sh1sh2 = float2ushort(ff)
printpair(*sh1sh2, ff)

ww = float2ushort(17.7)
print(ww)



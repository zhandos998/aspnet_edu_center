# numb=int(input())
# b=True
# i=2
# while numb>1:
#     if (numb%i==0):
#         numb=numb/i
#         # i=2
#         b=not b
#         # i+=1
#     else:
#         i+=1
#     # print(i)
# if not b:
#     print("Ikram")
# else:
#     print("Rakhman")

numb=int(input())
r = -1
for i in range(2,10**5):
    k=(i*i-1)/numb
    if k.is_integer() and k>=1:
        r=int(k)
        break
print(r)
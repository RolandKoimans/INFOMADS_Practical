with open('instance.txt', encoding="utf-8") as f:
    lines = f.readlines()
    for line in range(len(lines)):
        lines[line] = lines[line].strip()

p1 = int(lines[0])
p2 = int(lines[1])
g = int(lines[2])
np = int(lines[3])

with open('solution.txt', encoding="utf-8") as f:
    liness = f.readlines()
    for line in range(len(liness)):
        liness[line] = liness[line].strip()
rused = int(liness[0])

j = []
for b in range(4, 4 + np):
    patients = [int(x) for x in lines[b].split(',')]
    j.append(patients)

s = []
for c in range(1, 1 + np):
    patients = [int(x) for x in liness[c].split(',')]
    s.append(patients)

for cpatient in range(len(s)-1):
    for npatient in range(cpatient + 1, len(s)):
        if s[cpatient][1] == s[npatient][1]:
            if s[cpatient][0] <= s[npatient][0] <= s[cpatient][0]+p1-1:
                print("fail1")
                exit()
        if s[cpatient][1] == s[npatient][3]:
            if s[cpatient][0] <= s[npatient][2] <= s[cpatient][0]+p1-1:
                print("fail2")
                exit()
        if s[cpatient][3] == s[npatient][1]:
            if s[cpatient][2] <= s[npatient][0] <= s[cpatient][2]+p2-1:
                print("fail3")
                exit()
        if s[cpatient][3] == s[npatient][3]:
            if s[cpatient][2] <= s[npatient][2] <= s[cpatient][2]+p2-1:
                print("fail4")
                exit()
print("solution correct")


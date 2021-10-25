import gurobipy as gp
from gurobipy import GRB
import string

try:
    with open('instance.txt', encoding="utf-8") as f:
        lines = f.readlines()
        for line in range(len(lines)):
            lines[line] = lines[line].strip()
    
    # Create a new model
    m = gp.Model("vaccineschedule")

    p1 = int(lines[0])
    p2 = int(lines[1])
    g = int(lines[2])
    number_of_patients = int(lines[3])
    print(number_of_patients)
    j = []
    for b in range(4, 4 + number_of_patients):
        patients = [int(x) for x in lines[b].split(',')]
        j.append(patients)
    J = len(j)
    print("Patient lenght:",J)
    T = 0 
    R = J # Rooms at most is #patients
    W = 2 # At most 2 jabs

    for pa in range(len(j)):
        longest_time = j[pa][1] + j[pa][2] + g + p2 + j[pa][3]
        if longest_time > T:
            T = longest_time

    #j = [[38,49,1,27],[19,30,0,25],[19,30,0,25],[58,72,1,29],[10,22,0,26],[35,45,1,27],[1,13,0,28],[7,18,2,26],[34,47,2,26],[47,59,2,29],[3,16,0,27]]
    for a in range(len(j)):
        j[a][0] = j[a][0]-1
        j[a][1] = j[a][1]-1

    # Create variables
    c = m.addVar(vtype=GRB.INTEGER, name="c")
    roomsused = m.addVar(vtype=GRB.INTEGER, name="roomsused")
    x = m.addVars(J, T, R, W, vtype=GRB.BINARY, name="x")
    xstart = m.addVars(J, T, R, W, vtype=GRB.BINARY, name="xstart")
    xend = m.addVars(J, T, R, W, vtype=GRB.BINARY, name="xend")
    # Note: this is the starttime of the second jab even though we only create 1 here so it looks like w=1
    starttime = m.addVars(J, W, vtype=GRB.INTEGER, name="starttime")
    endtime = m.addVars(J, W, vtype=GRB.INTEGER, name="endtime")

    # Set objective
    m.setObjective(c + roomsused, GRB.MINIMIZE)

    # Add constraint
    for timeslot in range(T):
        m.addConstr((c >= x.sum('*', timeslot, '*', '*') ), name="concurrent room use")
    for timeslot in range(T):
        for room in range(R):
            m.addConstr((x.sum('*', timeslot, room, '*') <= 1 ), name="room occupancy at most one")
    # Processing time for jab 1
    for patient in range(J):
        m.addConstr((starttime[patient, 0] >= j[patient][0]))
        m.addConstr((endtime[patient, 0] <= j[patient][1]))
        #m.addConstr((x.sum(patient, [j[patient][0],j[patient][1]], '*', 0) == p1), name="processing of jab1 is p1 length")
    # Deze staat niet in overleaf, maar moet wel maybe? iig hier nodig
    for patient in range(J):
        m.addConstr((x.sum(patient, '*', '*', 0) == p1), name="processing at most p1 for jab1 over all T")

    # Processing time for jab 2
    for patient in range(J):
        patientgap = j[patient][2]
        gap = g
        m.addConstr(endtime[patient, 0] + patientgap + gap <= starttime[patient, 1] - 1)
        m.addConstr(endtime[patient, 0] + patientgap + gap + j[patient][3] >= starttime[patient, 1] + p2 - 1)
    for patient in range(J):
        m.addConstr((x.sum(patient, '*', '*', 1) == p2), name="processing at most p2 for jab2 over all T")

    #xstart
    for patient in range(J):
        for jab in range(W):
            for room in range(R):
                for timeslot in range(1, T):
                    m.addConstr((xstart[patient, timeslot, room, jab] >= x[patient, timeslot, room, jab] - x[patient, timeslot - 1, room, jab]), name="xstart")
                m.addConstr((xstart[patient, 0, room, jab] >= x[patient, 0, room, jab]), name="xstart start edge")
    #xstart at most one can be 1
    for patient in range(J):
        for jab in range(W):
            m.addConstr((xstart.sum(patient, '*', '*', jab) == 1), name="xstart sum to 1")
    #xend
    for patient in range(J):
        for jab in range(W):
            for room in range(R):
                for timeslot in range(T-1):
                    m.addConstr((xend[patient, timeslot, room, jab] >= x[patient, timeslot, room, jab] - x[patient, timeslot + 1, room, jab]), name="xend")
                m.addConstr((xend[patient, T-1, room, jab] >= x[patient, T-1, room, jab]), name="xstart end edge")
    #xend at most one can be 1
    for patient in range(J):
        for jab in range(W):
            m.addConstr((xend.sum(patient, '*', '*', jab) == 1), name="xend sum to 1")

    #starttime jab 2
    for patient in range(J):
        m.addConstr((starttime[patient, 0] == sum(timeslot * xstart.sum(patient, timeslot, '*', 0) for timeslot in range (T))))
        m.addConstr((starttime[patient, 1] == sum(timeslot * xstart.sum(patient, timeslot, '*', 1) for timeslot in range (T))))
        m.addConstr((endtime[patient, 0] == sum(timeslot * xend.sum(patient, timeslot, '*', 0) for timeslot in range (T))))
        m.addConstr((endtime[patient, 1] == sum(timeslot * xend.sum(patient, timeslot, '*', 1) for timeslot in range (T))))

    m.addConstr(roomsused == sum(room * x.sum('*', '*', room, '*') for room in range(R)))
    


    # Optimize model
    m.optimize()

    timeslotroom = [[0 for x in range(R)] for y in range(T)]
    patientroom1 = [0 for x in range(J)]
    patientroom2 = [0 for x in range(J)]
    patientstarttime1 =[0 for x in range(J)]
    patientstarttime2 =[0 for x in range(J)]
    patientendtime1 =[0 for x in range(J)]
    patientendtime2 =[0 for x in range(J)]

    roomsused = 0
    for v in m.getVars():
        if v.varName[:9] == "starttime":
            values = v.varName[10:]
            values = values[:-1]
            starttimes = [int(x) for x in values.split(',')]
            if starttimes[1] == 0:
                patientstarttime1[starttimes[0]] = int(v.x)
            if starttimes[1] == 1:
                patientstarttime2[starttimes[0]] = int(v.x)

        if v.varName[:7] == "endtime":
            values = v.varName[8:]
            values = values[:-1]
            endtimes = [int(x) for x in values.split(',')]
            if endtimes[1] == 0:
                patientendtime1[starttimes[0]] = int(v.x)
            if endtimes[1] == 1:
                patientendtime2[starttimes[0]] = int(v.x)
            
    for v in m.getVars():
        if v.varName == "c":
            roomsused = int(v.x)
            print("Rooms used: ", v.x)
        if v.varName[0] == "x":
            if v.varName[1] == "[":
                values = v.varName[2:]
                values = values[:-1]
                myvalues = [int(x) for x in values.split(',')]
                
                if (int(v.x)) > 0:
                    if patientstarttime2[myvalues[0]] > myvalues[1]:
                        patientroom1[myvalues[0]] = myvalues[2]
                    if patientstarttime2[myvalues[0]] <= myvalues[1]:
                        patientroom2[myvalues[0]] = myvalues[2]
    
    sol = open("solution.txt", "w")
    sol.write(str(roomsused)+"\n")

    for pat in range(J):
        stringetje = str(patientstarttime1[pat]+1) + "," + str(patientroom1[pat]+1)  + "," + str(patientstarttime2[pat]+1) + "," + str(patientroom2[pat]+1)
        print(stringetje)
        sol.write(stringetje + "\n")
    sol.close()

except gp.GurobiError as e:
    print('Error code ' + str(e.errno) + ': ' + str(e))

except AttributeError as e:
    print(e)
    print('Encountered an attribute error')

1. All wordings changed from 'Should' to 'MUST' since should is not an mandatory wording for agent
2. Removed un-wanted hours from Task.Json 
3. Skill set fine tuned to get consistent task number format in task.json file
4. Backend & Frontend Skill set modified to avoid test case file generations since that is job of test agent
5. Entity level changes needs to be updated as migration files automatically
6. Sysadmin username and password fed as through seed data
9. Created enum creation rule for consistent frontend and backend handshake
10. Skills fine tuned to avoid displaying primary key id from datatable rows
11. Skills modified for system lookup comparison
12. Strict forcement for theme color 
Frondend
---------
1) Provided skillset to generate comments for components/functions/events etc
2) Skill fine tuned to generate interceptor for front-end global error handling
3) Skill optimised for folder structure separation based on application/ module / area /routing /interface/ constants, enum / custom configuration and css
4) Skill rule enforcement for binding IDs in Dropdowns
 
Backend
-------
1) Skill modified to handle project name based rootfolder creations
2) Skill modified to strictly follow API folder structure layer
3) Enforced all endpoint to use communication using content-type as application/json
4) Skill rule enforcement for response json strcture
5) Skills modified to set global error handling 
6) Enforced to provide bat files for run the application from root folder
7) API port number enforment through skill rules

- @agent-requirements 12k (5m 15s)
- @agent-architect "Create v1 requirements" 14.5k (3m 39s)
- @agent-planner "Create v1 architecture" 18.3k (4m 2s)
- @agent-backend "Execute v1 plan" 95k (36m 31s)
- @agent-frontend "Execute v1 plan" 48k (30m) 
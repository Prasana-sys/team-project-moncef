# MonCal
## _The Best calender, Ever_
<img src="https://github.com/EECE3093C/team-project-moncef/blob/main/MonCal%20logo.png" width="150" height="112.50">

MonCal is a **Centralized Calendar Management Desktop Application** that integrates with popular calendar platforms, such as Gmail, Outlook, and Apple, primarily developed for students in a college setting. The application will allow users to view all their calendar events in a centralized hub and create new events with advanced scheduling options, such as weekly repetition or custom recurrence patterns. Other competitors have desktop calendar applications which are clunky and have outdated interfaces.

### Build Notes:
#### v0.2.0:

```mermaid
graph TD;
    MonCal Calendar -->|uses| Microsoft Graph API;
    MonCal Calendar -->|uses| Google Calendar API;
    MonCal Calendar -->|has| CalendarGUI;
 ```
    
#####  Calendar Integration:
![CalIntegration](https://github.com/EECE3093C/team-project-moncef/blob/main/docs/MonCal%20API%20Permissions.png)
![Outlook Integration](https://github.com/EECE3093C/team-project-moncef/blob/main/docs/MSGraph_token_access.png)

##### GUI:
![v0.2.0 GUI](https://github.com/EECE3093C/team-project-moncef/blob/main/v0.2.0%20GUI.png)







The project's goal is to simplify calendar management for users who use multiple calendar platforms. Currently, users who have multiple calendars often have to switch between different platforms and applications to view their events, leading to a disjointed and confusing experience. MonCal will eliminate this issue by providing users with a single, unified view of their calendar events.

The application will be developed using popular programming languages such as C#/C++, and will incorporate a user-friendly interface to make it easy for users to interact with their calendar data. The application will also use APIs from popular calendar platforms to access and manage user calendar data, allowing users to easily connect their calendars to the application. The main components/modules that will interact are the calendar platform APIs, the calendar GUI, and event scheduling modules. Data will be stored and accessed locally and will contain user event data and account information.

The single most serious challenge we see in developing this product is successfully integrating popular calendar platforms' APIs to our app. To minimize this risk we will conduct extensive research and thoroughly study the API's documentation. Other challenges we might face include security breaches and unforeseen technical issues while implementing our idea. 

We are planning on adding features such as:
- User interface customization
- Collaboration tools: tools for users to collaborate with others, such as the ability to share calendars and events with friends, family, or colleagues.
- Adding assignment due dates/deadlines to calendars
- Ability to create an event for colloborating on a project/assignment that links to a specified document on Google Drive/OneDrive

README Ver 2.0



# Vertice
## 3D Archive Interface

![Vertice Logo](https://mir-s3-cdn-cf.behance.net/project_modules/1400/2505ce60149489.5a3e10aad8365.png)

## Overview
Vertice is a platform that enables cultural institutions and their visitors to collate and interact with digitised and born-digital 3D objects. Vertice was developed using the gaming engine Unity to provide functionality that supports the purposeful arrangement, contextualisation, and presentation of digital artefacts.

Through considered interaction design and incorporation of gamification, Vertice anticipates how three-dimensional space can alter and enhance the way users interact with intangible cultural artefacts.

Vertice is no longer actively maintained, however, forking or contributions to this repository are always welcome.

## Scenes
Vertice is comprised of three different scenes; entitled _Import_, _Browse_, and _Collections_.

![Vertice Value Chain](https://mir-s3-cdn-cf.behance.net/project_modules/1400/50799260149489.5a3e122595e26.png)

## Import Scene
> Accessions an artefact’s 3D model, and associates it with contextual metadata that attests to its authenticity and cultural significance.
> An imported artefact will be available for browsing and organisation into collections.

![Empty Import Scene](https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/f524ee60149489.5a3e14b34a016.png)

### Metadata
A key aspect of this phase of research and development was designing and implementing a more effective means of describing 3D digital artefacts. While a number of precedents for achieving this were explored, the resulting approach leaned upon the Dublin Core Metadata Element Set, which was extended to encompass details specific to digital three-dimensional cultural artefacts.

![Metadata Structure](https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/54ae0360149489.5a3e152382231.png)
Empty Import scene on landing

![Import scene populated](https://mir-s3-cdn-cf.behance.net/project_modules/1400/10173160149489.5a3e14b34a603.png)
Import scene once model and texture have been imported and metadata assigned using the GUI dialogue boxes


## Browse Scene
> Information seeking for traditional media often takes place in two-dimensional space.
> Due to the three-dimensional nature of its content, Vertice reexamines this process and allows users to explore artefacts in a 3D environment that is idiomatic of a first-person video game.

![Browse scene empty](https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/9ea53a60149489.5a3e178a9e478.png)
Dialogue box in the top left corner is used to pick a certain metadata attribute to browse, or for text-based searching

![Browse scene populated](https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/b7909060149489.5a3e178a9f477.png)
Upon approaching an artefact of interest, the user is presented with contextual information and media describing the artefact. The user can collapse or expand information accordingly. The user can also add the artefact to their own collection for later reference.

![Browse scene metadata fields](https://mir-s3-cdn-cf.behance.net/project_modules/1400/cc944260149489.5a3e178a9ebe4.png)

## Collection Scene
> Allows curators and visitors to group artefacts according to their own system of arrangement.
> This allows curators to preserve spatial relationships between artefacts, and prepare virtual exhibitions.
> It is also a space where could also allow visitors can gather their favourite artefacts.

![Collection scene](https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/85feba60149489.5a3e178a9d8c8.png)
The Collection scene contains similarities in its interaction design to the Browse scene, where the user can move around in first person and approach artefacts. Each collection also contains its own metadata, housed in a similar approach to the artefact metadata schema

![Collection navigation](https://mir-s3-cdn-cf.behance.net/project_modules/1400/4de72060149489.5a3e178a9d5ce.png)
A key point of difference of the Collection scene is that it allows its users customisation for how artefacts are represented in their scene. Users can customise the scale, position and rotation of the artefacts in the collection without altering their depiction in the wider archive

![Collection scene](https://mir-s3-cdn-cf.behance.net/project_modules/1400/0dc75160149489.5a446a913d09d.png)

## Credits
* Design & Unity Development: Ryan Achten
* Web & Backend Development: Tim Bathgate
* Mentorship: Michael Elwood-Smith
* Supervision: Walter Langelaar
* Additional support from Professor Simon Fraser and [MADE, Victoria University of Wellington](https://made.ac.nz/about-made/)

Funded by VicLink & KiwiNet, 2016

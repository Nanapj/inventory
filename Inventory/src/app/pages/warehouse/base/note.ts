import { NoteDetail } from './note-detail';

export class Note {
    Id: string;
    Code: string;
    Noted: string;
    SourceId: string;
    DestinationId: string;
    SourceWarehouse: string;
    DestinationWarehouse: string;
    Total: number;
    ProductType: number;
    Details: NoteDetail[];
    CreatedDate: Date;
    
}
